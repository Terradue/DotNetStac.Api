using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;
using Stac.Api.Interfaces;
using Stac.Api.Models.Cql2;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;
using Stars.Geometry.NTS;

namespace Stac.Api.Models.Cql2
{
    public static class Cql2FilterTranslator
    {

        public static StacQueryable<TSource> WithCQL2<TSource>(this StacQueryable<TSource> source, BooleanExpression booleanExpression) where TSource : IStacObject
        {
            AndOrExpression andOrExpression = booleanExpression.AndOr();
            if (andOrExpression != null)
            {
                return source.WithCQL2(andOrExpression);
            }
            NotExpression notExpression = booleanExpression.Not();
            if (notExpression != null)
            {
                return source.WithCQL2(notExpression);
            }
            ComparisonPredicate comparisonPredicate = booleanExpression.Comparison();
            if (comparisonPredicate != null)
            {
                return source.WithCQL2(comparisonPredicate);
            }

            throw new NotImplementedException();
        }

        public static StacQueryable<TSource> WithCQL2<TSource>(this StacQueryable<TSource> source, AndOrExpression andOrExpression) where TSource : IStacObject
        {
            IQueryable<TSource> newSource = source.WithCQL2(andOrExpression.Args[0]);
            int i = 1;
            while (andOrExpression.Args.Count > i)
            {
                switch (andOrExpression.Op)
                {
                    case AndOrExpressionOp.And:
                        newSource = newSource.Intersect(source.WithCQL2(andOrExpression.Args[i]));
                        break;
                    case AndOrExpressionOp.Or:
                        newSource = newSource.Union(source.WithCQL2(andOrExpression.Args[i]));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                i++;
            }

            return new StacQueryable<TSource>(source.Provider as StacQueryProvider, newSource.Expression);
        }

        public static StacQueryable<TSource> WithCQL2<TSource>(this StacQueryable<TSource> source, NotExpression notExpression) where TSource : IStacObject
        {
            return new StacQueryable<TSource>(source.Provider as StacQueryProvider, source.WithCQL2(notExpression.Args[0]).Except(source).Expression);
        }

        public static StacQueryable<TSource> WithCQL2<TSource>(this StacQueryable<TSource> source, ComparisonPredicate comparisonPredicate) where TSource : IStacObject
        {
            return source.WhereCQL2(comparisonPredicate);
        }

        public static StacQueryable<TSource> WhereCQL2<TSource>(this StacQueryable<TSource> source,
                                                                  ComparisonPredicate binaryComparisonPredicate) where TSource : IStacObject
        {
            // This method creates a lambda expression that compares the 2 arguments
            // As we start a lambda expression, we need to create a parameter
            ParameterExpression itemParameter = Expression.Parameter(typeof(TSource), "item");
            ParameterExpression providerParameter = Expression.Parameter(typeof(IStacQueryProvider), "provider");

            Expression body = CQL2ComparisonToExpression<TSource>(binaryComparisonPredicate, itemParameter, providerParameter);
            var lambda = Expression.Lambda<Func<TSource, bool>>(body, itemParameter);

            var whereMethod = typeof(Queryable).GetMethods()
                .Where(m => m.Name == nameof(Queryable.Where)
                            && m.GetParameters().Length == 2)
                .Where(p => p.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                            && p.GetParameters().Last().ParameterType.GetGenericTypeDefinition() == typeof(Expression<>)
                            // Ensure Func has 2 args
                            && p.GetParameters().Last().ParameterType.GetGenericArguments().First().GetGenericArguments().Length == 2)
                .Single();

            var callExpression = Expression.Call(
                    null,
                    whereMethod.MakeGenericMethod(typeof(TSource)),
                    new Expression[] { source.Expression, Expression.Quote(lambda) }
                    );

            return new StacQueryable<TSource>(source.Provider as IStacQueryProvider,
                                              callExpression);

        }

        private static Expression CQL2ComparisonToExpression<TSource>(ComparisonPredicate binaryComparisonPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            switch (binaryComparisonPredicate)
            {
                case BinaryComparisonPredicate binaryComparisonPredicate1:
                    return CQL2BinaryComparisonToExpression<TSource>(binaryComparisonPredicate1, itemParameter, providerParameter);
                case IsLikePredicate isLikePredicate:
                    return CQL2ToIsLikeToExpression<TSource>(isLikePredicate, itemParameter, providerParameter);
                case IsBetweenPredicate isBetweenPredicate:
                    return CQL2ToIsBetweenToExpression<TSource>(isBetweenPredicate, itemParameter, providerParameter);
                case IsInListPredicate isInListPredicate:
                    return CQL2ToIsInListToExpression<TSource>(isInListPredicate, itemParameter, providerParameter);
                case IsNullPredicate isNullPredicate:
                    return CQL2ToIsNullToExpression<TSource>(isNullPredicate, itemParameter, providerParameter);
                case SpatialPredicate spatialPredicate:
                    return CQL2ToSpatialToExpression<TSource>(spatialPredicate, itemParameter, providerParameter);
                default:
                    throw new NotImplementedException(binaryComparisonPredicate.GetType().Name);
            }
        }



        private static BinaryExpression CQL2BinaryComparisonToExpression<TSource>(BinaryComparisonPredicate binaryComparisonPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var left = CQL2ToExpression<TSource>(binaryComparisonPredicate.Args[0], itemParameter, providerParameter);
            var right = CQL2ToExpression<TSource>(binaryComparisonPredicate.Args[1], itemParameter, providerParameter);

            var methodInfo = typeof(IComparable).GetMethod("CompareTo", new Type[] { typeof(object) });
            var compareExpression = Expression.Call(left, methodInfo, right);
            var zeroConst = Expression.Constant(0, typeof(Int32));

            switch (binaryComparisonPredicate.Op)
            {
                case ComparisonPredicateOp.Eq:
                    return Expression.Equal(compareExpression, zeroConst);
                case ComparisonPredicateOp.Diff:
                    return Expression.NotEqual(compareExpression, zeroConst);
                case ComparisonPredicateOp.Lt:
                    return Expression.LessThan(compareExpression, zeroConst);
                case ComparisonPredicateOp.Le:
                    return Expression.LessThanOrEqual(compareExpression, zeroConst);
                case ComparisonPredicateOp.Gt:
                    return Expression.GreaterThan(compareExpression, zeroConst);
                case ComparisonPredicateOp.Ge:
                    return Expression.GreaterThanOrEqual(compareExpression, zeroConst);
            }

            throw new NotImplementedException();
        }

        private static Expression CQL2ToIsLikeToExpression<TSource>(IsLikePredicate isLikeComparisonPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var left = CQL2ToExpression<TSource>(isLikeComparisonPredicate.Args[0], itemParameter, providerParameter);
            var right = CQL2ToExpression<TSource>(isLikeComparisonPredicate.Args[1], itemParameter, providerParameter);

            var methodInfo = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
            var rightAsRegex = Expression.Call(null, typeof(ItemFiltersExtensions).GetMethod("LikeToRegex", new Type[] { typeof(IComparable) }), right);
            var isLikeExpression = Expression.Call(rightAsRegex, methodInfo, Expression.Call(left, typeof(object).GetMethod("ToString", new Type[] { })));

            return isLikeExpression;

        }

        private static BinaryExpression CQL2ToIsBetweenToExpression<TSource>(IsBetweenPredicate isBetweenPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var main = CQL2ToExpression<TSource>(isBetweenPredicate.Args[0], itemParameter, providerParameter);
            var min = CQL2ToExpression<TSource>(isBetweenPredicate.Args[1], itemParameter, providerParameter);
            var max = CQL2ToExpression<TSource>(isBetweenPredicate.Args[2], itemParameter, providerParameter);

            var methodInfo = typeof(IComparable).GetMethod("CompareTo", new Type[] { typeof(object) });
            var minCompareExpression = Expression.Call(main, methodInfo, min);
            var maxCompareExpression = Expression.Call(main, methodInfo, max);
            var zeroConst = Expression.Constant(0, typeof(Int32));

            return Expression.AndAlso(Expression.GreaterThanOrEqual(minCompareExpression, zeroConst), Expression.LessThanOrEqual(maxCompareExpression, zeroConst));
        }

        private static BinaryExpression CQL2ToIsInListToExpression<TSource>(IsInListPredicate isInListPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var main = CQL2ToExpression<TSource>(isInListPredicate.Args[0], itemParameter, providerParameter);

            var methodInfo = typeof(IComparable).GetMethod("CompareTo", new Type[] { typeof(object) });
            var zeroConst = Expression.Constant(0, typeof(Int32));

            var orExpressions = new List<Expression>();
            foreach (var arg in isInListPredicate.Args.Skip(1))
            {
                var argExpression = CQL2ToExpression<TSource>(arg, itemParameter, providerParameter);
                var compareExpression = Expression.Call(main, methodInfo, argExpression);
                orExpressions.Add(Expression.Equal(compareExpression, zeroConst));
            }

            return Expression.OrElse(orExpressions[0], orExpressions[1]);
        }

        private static Expression CQL2ToIsNullToExpression<TSource>(IsNullPredicate isNullPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var main = CQL2ToExpression<TSource>(isNullPredicate.Args, itemParameter, providerParameter);

            var methodInfo = typeof(object).GetMethod("Equals", new Type[] { typeof(object) });
            var nullConst = Expression.Constant(null, typeof(object));
            var compareExpression = Expression.Call(main, methodInfo, nullConst);

            return Expression.IsTrue(compareExpression);
        }

        private static Expression CQL2ToSpatialToExpression<TSource>(SpatialPredicate spatialPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var left = CQL2ToExpression<TSource>(spatialPredicate.Args[0], itemParameter, providerParameter);
            var right = CQL2ToExpression<TSource>(spatialPredicate.Args[1], itemParameter, providerParameter);
            MethodInfo methodInfo = GetSpatialMethod(spatialPredicate.Op);
            var spatialExpression = Expression.Call(providerParameter, methodInfo, left, right);

            return spatialExpression;
        }

        private static MethodInfo GetSpatialMethod(SpatialPredicateOp op)
        {
            switch (op)
            {
                case SpatialPredicateOp.S_contains:
                    return typeof(IStacQueryProvider).GetMethod("SpatialContains", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_crosses:
                    return typeof(IStacQueryProvider).GetMethod("SpatialCrosses", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_disjoint:
                    return typeof(IStacQueryProvider).GetMethod("SpatialDisjoint", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_equals:
                    return typeof(IStacQueryProvider).GetMethod("SpatialEquals", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_intersects:
                    return typeof(IStacQueryProvider).GetMethod("SpatialIntersects", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_overlaps:
                    return typeof(IStacQueryProvider).GetMethod("SpatialOverlaps", new Type[] { typeof(Geometry), typeof(Geometry) });
                case SpatialPredicateOp.S_touches:
                    return typeof(IStacQueryProvider).GetMethod("SpatialTouches", new Type[] { typeof(Geometry), typeof(Geometry) });
            }
            throw new NotImplementedException(op.GetType().ToString());
        }

        public static Expression CQL2ToExpression<TSource>(IScalarExpression scalarExpression, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (scalarExpression is IScalarLiteral scalarLiteral)
            {
                return Expression.Constant(scalarLiteral.Value, typeof(IComparable));
            }

            if (scalarExpression is CharExpression charExpression)
            {
                return CQL2ToExpression<TSource>(charExpression, item, providerParameter);
            }

            throw new NotImplementedException();
        }

        public static Expression CQL2ToExpression<TSource>(IInListOperand inListOperand, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (inListOperand is IScalarExpression scalarExpression)
            {
                return CQL2ToExpression<TSource>(scalarExpression, item, providerParameter);
            }

            throw new NotImplementedException();
        }

        public static Expression CQL2ToExpression<TSource>(IIsNullOperand isNullOperand, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (isNullOperand is IScalarExpression scalarExpression)
            {
                return CQL2ToExpression<TSource>(scalarExpression, item, providerParameter);
            }

            throw new NotImplementedException();
        }

        public static Expression CQL2ToExpression<TSource>(INumericExpression numericExpression, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (numericExpression is Number number)
            {
                return Expression.Constant(number.Value, typeof(IComparable));
            }
            if (numericExpression is CharExpression charExpression)
            {
                return CQL2ToExpression<TSource>(charExpression, item, providerParameter);
            }
            throw new NotImplementedException();
        }

        public static Expression CQL2ToExpression<TSource>(CharExpression charExpression, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            PropertyRef propertyRef = charExpression.Property();
            if (propertyRef != null)
            {
                var method = typeof(IStacQueryProvider).GetMethod("GetStacObjectProperty").MakeGenericMethod(typeof(TSource));
                return Expression.Call(providerParameter,
                                       method,
                                       item, Expression.Constant(propertyRef.Property));
            }

            String str = charExpression.String();
            if (str != null)
            {
                return Expression.Constant(str.Str, typeof(IComparable));
            }

            throw new NotImplementedException(charExpression.GetType().Name);
        }

        public static Expression CQL2ToExpression<TSource>(IGeomExpression geomExpression, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (geomExpression is ISpatialLiteral spatialLiteral)
            {
                return CQL2ToExpression<TSource>(spatialLiteral, item, providerParameter);
            }

            if (geomExpression is PropertyRef propertyRef)
            {
                var method = typeof(StacQueryProvider).GetMethod("GetStacObjectGeometry").MakeGenericMethod(typeof(TSource));
                return Expression.Call(providerParameter,
                                       method,
                                       item, Expression.Constant(propertyRef.Property));
            }

            throw new NotImplementedException(geomExpression.GetType().Name);
        }

        public static Expression CQL2ToExpression<TSource>(ISpatialLiteral spatialLiteral, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (spatialLiteral is GeometryLiteral geometryLiteral)
            {
                return Expression.Constant(geometryLiteral.GeometryObject.ToNTSGeometry(), typeof(Geometry));
            }
            if (spatialLiteral is EnvelopeLiteral envelopeLiteral)
            {
                return Expression.Constant(new Envelope(envelopeLiteral.Bbox[0], envelopeLiteral.Bbox[2], envelopeLiteral.Bbox[1], envelopeLiteral.Bbox[3]), typeof(Geometry));
            }

            throw new NotImplementedException(spatialLiteral.GetType().Name);
        }

    }
}
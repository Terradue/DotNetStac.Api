using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using GeoJSON.Net.Geometry;
using Itenso.TimePeriod;
using NetTopologySuite.Geometries;
using Stac.Api.Interfaces;
using Stac.Api.Models.Cql2;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;
using Stars.Geometry.NTS;

namespace Stac.Api.Models.Cql2
{
    public static class Cql2Linq
    {

        public static IEnumerable<TSource> Boolean<TSource>(this IEnumerable<TSource> source, BooleanExpression booleanExpression) where TSource : IStacObject
        {
            return source.AsQueryable().Boolean(booleanExpression);
        }

        public static IQueryable<TSource> Boolean<TSource>(this IQueryable<TSource> source, BooleanExpression booleanExpression) where TSource : IStacObject
        {
            // This method creates a lambda expression that return a boolean value
            // As we start a lambda expression, we need to create a parameter
            ParameterExpression itemParameter = Expression.Parameter(typeof(TSource), "item");
            ParameterExpression providerParameter = Expression.Parameter(typeof(IStacQueryProvider), "provider");

            // We need to create a body for the lambda expression with all the rest of the filter
            Expression body = CQL2BooleanToExpression<TSource>(booleanExpression, itemParameter, providerParameter);
            var lambda = Expression.Lambda<Func<TSource, bool>>(body, itemParameter);

            // Get the Where method from the IQueryable interface
            var whereMethod = typeof(Queryable).GetMethods()
                .Where(m => m.Name == nameof(Queryable.Where)
                            && m.GetParameters().Length == 2)
                .Where(p => p.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                            && p.GetParameters().Last().ParameterType.GetGenericTypeDefinition() == typeof(Expression<>)
                            // Ensure Func has 2 args
                            && p.GetParameters().Last().ParameterType.GetGenericArguments().First().GetGenericArguments().Length == 2)
                .Single();

            // Call the Where method with the lambda expression
            var callExpression = Expression.Call(
                    null,
                    whereMethod.MakeGenericMethod(typeof(TSource)),
                    new Expression[] { source.Expression, Expression.Quote(lambda) }
                    );

            // Return a new StacQueryable with the new expression
            return new StacQueryable<TSource>(source.Provider as IStacQueryProvider,
                                              callExpression);
        }

        public static Expression CQL2BooleanToExpression<TSource>(BooleanExpression booleanExpression, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            AndOrExpression andOrExpression = booleanExpression.AndOrExpression();
            if (andOrExpression != null)
            {
                return CQL2AndOrToExpression<TSource>(andOrExpression, itemParameter, providerParameter);
            }
            NotExpression notExpression = booleanExpression.NotExpression();
            if (notExpression != null)
            {
                return CQL2NotToExpression<TSource>(notExpression, itemParameter, providerParameter);
            }
            ComparisonPredicate comparisonPredicate = booleanExpression.Comparison();
            if (comparisonPredicate != null)
            {
                return CQL2ComparisonToExpression<TSource>(comparisonPredicate, itemParameter, providerParameter);
            }

            throw new NotImplementedException();
        }

        public static Expression CQL2AndOrToExpression<TSource>(AndOrExpression andOrExpression, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // check the arguments
            if (andOrExpression.Args.Count < 2)
            {
                throw new ArgumentException("AndOrExpression must have at least 2 arguments");
            }

            // We pass the parameter to the CQL2BooleanToExpression method to get left and right expressions
            var left = CQL2BooleanToExpression<TSource>(andOrExpression.Args[0], itemParameter, providerParameter);
            var right = CQL2BooleanToExpression<TSource>(andOrExpression.Args[1], itemParameter, providerParameter);

            switch (andOrExpression.Op)
            {
                case AndOrExpressionOp.And:
                    return Expression.AndAlso(left, right);
                case AndOrExpressionOp.Or:
                    return Expression.OrElse(left, right);
                default:
                    throw new NotImplementedException();
            }

        }

        public static Expression CQL2NotToExpression<TSource>(NotExpression notExpression, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // check the arguments
            if (notExpression.Args.Count != 1)
            {
                throw new ArgumentException("NotExpression must have exactly 1 argument");
            }

            // We pass the parameter to the CQL2BooleanToExpression method to get the negated expression
            var negated = CQL2BooleanToExpression<TSource>(notExpression.Args[0], itemParameter, providerParameter);

            return Expression.Not(negated);
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
                case TemporalPredicate temporalPredicate:
                    return CQL2ToTemporalToExpression<TSource>(temporalPredicate, itemParameter, providerParameter);
                default:
                    throw new NotImplementedException(binaryComparisonPredicate.GetType().Name);
            }
        }

        private static BinaryExpression CQL2BinaryComparisonToExpression<TSource>(BinaryComparisonPredicate binaryComparisonPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // check the arguments
            if (binaryComparisonPredicate.Args.Count != 2)
            {
                throw new ArgumentException("BinaryComparisonPredicate must have exactly 2 arguments");
            }

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
            var isLikeExpression = Expression.Call(null, typeof(ItemFiltersExtensions).GetMethod("IsLike", new Type[] { typeof(object), typeof(IComparable) }), left, right);

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

        private static Expression CQL2ToTemporalToExpression<TSource>(TemporalPredicate temporalPredicate, ParameterExpression itemParameter, ParameterExpression providerParameter) where TSource : IStacObject
        {
            // We pass the parameter to the CQL2ToExpression method to get the scalar expression value
            var left = CQL2ToExpression<TSource>(temporalPredicate.Args[0], itemParameter, providerParameter);
            var right = CQL2ToExpression<TSource>(temporalPredicate.Args[1], itemParameter, providerParameter);
            MethodInfo methodInfo = GetTemporalMethod(temporalPredicate.Op);
            var temporalExpression = Expression.Call(providerParameter, methodInfo, left, right);

            return temporalExpression;
        }

        private static MethodInfo GetTemporalMethod(TemporalPredicateOp op)
        {
            switch (op)
            {
                case TemporalPredicateOp.T_before:
                    return typeof(IStacQueryProvider).GetMethod("TemporalBefore", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_during:
                    return typeof(IStacQueryProvider).GetMethod("TemporalDuring", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_equals:
                    return typeof(IStacQueryProvider).GetMethod("TemporalEquals", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_meets:
                    return typeof(IStacQueryProvider).GetMethod("TemporalMeets", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_overlaps:
                    return typeof(IStacQueryProvider).GetMethod("TemporalOverlaps", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_starts:
                    return typeof(IStacQueryProvider).GetMethod("TemporalStarts", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
                case TemporalPredicateOp.T_intersects:
                    return typeof(IStacQueryProvider).GetMethod("TemporalIntersects", new Type[] { typeof(ITimePeriod), typeof(ITimePeriod) });
            }
            throw new NotImplementedException(op.ToString());
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
            if (isNullOperand is CharExpression charExpression)
            {
                return CQL2ToExpression<TSource>(charExpression, item, providerParameter);
            }
            if (isNullOperand is Number number)
            {
                return Expression.Constant(number.Value, typeof(IComparable));
            }
            if (isNullOperand is ITemporalExpression temporalExpression)
            {
                return CQL2ToExpression<TSource>(temporalExpression, item, providerParameter);
            }
            if (isNullOperand is BooleanExpression booleanExpression)
            {
                return CQL2BooleanToExpression<TSource>(booleanExpression, item, providerParameter);
            }
            if (isNullOperand is IGeomExpression geomExpression)
            {
                return CQL2ToExpression<TSource>(geomExpression, item, providerParameter);
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

        public static Expression CQL2ToExpression<TSource>(ITemporalExpression temporalExpression, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (temporalExpression is ITemporalLiteral temporalLiteral)
            {
                return CQL2ToExpression<TSource>(temporalLiteral, item, providerParameter);
            }
            if (temporalExpression is PropertyRef propertyRef)
            {
                var method = typeof(IStacQueryProvider).GetMethod("GetStacObjectDateTime").MakeGenericMethod(typeof(TSource));
                return Expression.Call(providerParameter,
                                       method,
                                       item, Expression.Constant(propertyRef.Property));
            }
            throw new NotImplementedException();
        }

        public static Expression CQL2ToExpression<TSource>(ITemporalLiteral temporalLiteral, ParameterExpression item, ParameterExpression providerParameter) where TSource : IStacObject
        {
            if (temporalLiteral is InstantLiteral instantLiteral)
            {
                return Expression.Constant(new TimeInterval(instantLiteral.DateTime.DateTime, instantLiteral.DateTime.DateTime));
            }
            if (temporalLiteral is IntervalLiteral intervalLiteral)
            {
                return Expression.Constant(intervalLiteral.TimeInterval);
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
                var method = typeof(IStacQueryProvider).GetMethod("GetStacObjectGeometry").MakeGenericMethod(typeof(TSource));
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
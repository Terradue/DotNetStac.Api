using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Interfaces;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Services.Filtering
{
    public static class ItemFiltersExtensions
    {
        public static IEnumerable<StacItem> Filter(this IEnumerable<StacItem> source, IStacFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter is BooleanExpression booleanExpression)
            {
                return source.Where(i => i.Filter(booleanExpression));
            }

            throw new NotSupportedException($"Filter type {filter.GetType().Name} is not supported");
        }

        public static bool Filter(this StacItem item, BooleanExpression be)
        {
            if (be == null)
            {
                return true;
            }

            AndOrExpression andOrExpression = be.AndOr();
            if (andOrExpression != null)
            {
                return item.Filter(andOrExpression);
            }

            NotExpression notExpression = be.Not();
            if (notExpression != null)
            {
                return item.Filter(notExpression);
            }

            ComparisonPredicate comparisonPredicate = be.Comparison();
            if (comparisonPredicate != null)
            {
                return item.Filter(comparisonPredicate);
            }

            SpatialPredicate spatialPredicate = be.Spatial();
            if (spatialPredicate != null)
            {
                return item.Filter(spatialPredicate);
            }

            TemporalPredicate temporalPredicate = be.Temporal();
            if (temporalPredicate != null)
            {
                return item.Filter(temporalPredicate);
            }

            ArrayPredicate arrayPredicate = be.Array();
            if (arrayPredicate != null)
            {
                return item.Filter(arrayPredicate);
            }

            return true;
        }

        public static bool Filter(this StacItem item, AndOrExpression aoe)
        {
            if (aoe == null)
            {
                return true;
            }

            switch (aoe.Op)
            {
                case AndOrExpressionOp.And:
                    return aoe.Args.All(a => item.Filter(a));
                case AndOrExpressionOp.Or:
                    return aoe.Args.Any(a => item.Filter(a));
                default:
                    throw new NotSupportedException($"Operator {aoe.Op} is not supported");
            }
        }

        public static bool Filter(this StacItem item, NotExpression ne)
        {
            if (ne == null)
            {
                return true;
            }

            switch (ne.Op)
            {
                case NotExpressionOp.Not:
                    return !item.Filter(ne.Args[0]);
                default:
                    throw new NotSupportedException($"Operator {ne.Op} is not supported");
            }
        }

        public static bool Filter(this StacItem item, ComparisonPredicate cp)
        {
            if (cp == null)
            {
                return true;
            }

            BinaryComparisonPredicate binaryComparisonPredicate = cp.Binary();
            if (binaryComparisonPredicate != null)
            {
                return item.Filter(binaryComparisonPredicate);
            }

            IsLikePredicate isLikePredicate = cp.IsLike();
            if (isLikePredicate != null)
            {
                return item.Filter(isLikePredicate);
            }

            IsBetweenPredicate isBetweenPredicate = cp.IsBetween();
            if (isBetweenPredicate != null)
            {
                return item.Filter(isBetweenPredicate);
            }

            IsInListPredicate isInListPredicate = cp.IsInList();
            if (isInListPredicate != null)
            {
                return item.Filter(isInListPredicate);
            }

            IsNullPredicate isNullPredicate = cp.IsNull();
            if (isNullPredicate != null)
            {
                return item.Filter(isNullPredicate);
            }

            SpatialPredicate spatialPredicate = cp.Spatial();
            if (spatialPredicate != null)
            {
                return item.Filter(spatialPredicate);
            }

            TemporalPredicate temporalPredicate = cp.Temporal();
            if (temporalPredicate != null)
            {
                return item.Filter(temporalPredicate);
            }

            return true;
        }

        public static bool Filter(this StacItem item, BinaryComparisonPredicate cp)
        {
            if (cp == null)
            {
                return true;
            }

            IComparable left = GetValue(item, cp.Args[0]);
            if (left == null)
            {
                return false;
            }
            IComparable right = GetValue(item, cp.Args[1]);
            if (right == null)
            {
                return false;
            }

            switch (cp.Op)
            {
                case ComparisonPredicateOp.Eq:
                    return left.CompareTo(right) == 0;
                case ComparisonPredicateOp.Diff:
                    return left.CompareTo(right) != 0;
                case ComparisonPredicateOp.Ge:
                    return left.CompareTo(right) >= 0;
                case ComparisonPredicateOp.Gt:
                    return left.CompareTo(right) > 0;
                case ComparisonPredicateOp.Le:
                    return left.CompareTo(right) <= 0;
                case ComparisonPredicateOp.Lt:
                    return left.CompareTo(right) < 0;
                default:
                    throw new NotSupportedException($"Operator {cp.Op} is not supported");
            }
        }

        private static IComparable GetValue(StacItem item, IScalarExpression scalarExpression)
        {
            IComparable value = null;

            if (scalarExpression is BooleanExpression booleanExpression)
            {
                return Filter(item, booleanExpression);
            }

            if (scalarExpression is CharExpression charExpression)
            {
                return GetValue(item, charExpression);
            }

            if (scalarExpression is ITemporalInstantExpression temporalInstantExpression)
            {
                return temporalInstantExpression.DateTime;
            }

            if (scalarExpression is BoolExpression boolExpression)
            {
                return boolExpression.Bool;
            }

            if (scalarExpression is Number number)
            {
                return number.Num;
            }

            if (scalarExpression is IScalarLiteral scalarLiteral)
            {
                return GetValue(item, scalarLiteral);
            }

            return scalarExpression.ToString();
        }

        private static IComparable GetValue(StacItem item, CharExpression charExpression)
        {
            var casei = charExpression.Casei();
            if (casei != null)
            {
                return GetValue(item, casei);
            }

            var accenti = charExpression.Accenti();
            if (accenti != null)
            {
                return GetValue(item, accenti);
            }

            var propertyRef = charExpression.Property();
            if (propertyRef != null)
            {
                return GetValue(item, propertyRef);
            }

            var @string = charExpression.String();
            if (@string != null)
            {
                return GetValue(item, @string);
            }

            var function = charExpression.Function();
            if (function != null)
            {
                return GetValue(item, function);
            }

            throw new NotSupportedException($"CharExpression {charExpression} is not supported");
        }

        private static IComparable GetValue(StacItem item, CaseiExpression casei)
        {
            return GetValue(item, casei.Casei).ToString().ToLower();
        }

        private static IComparable GetValue(StacItem item, AccentiExpression accenti)
        {
            return GetValue(item, accenti.Accenti).ToString().ToLowerInvariant();
        }

        private static IComparable GetValue(StacItem item, PropertyRef propertyRef)
        {
            JToken jToken = JsonConvert.DeserializeObject<JToken>(StacConvert.Serialize(item));
            if (jToken.SelectToken(propertyRef.Property) == null)
            {
                return null;
            }
            IComparable value = jToken.SelectToken(propertyRef.Property).Value<IComparable>();
            value = NormalizeValue(value);
            return value;
        }

        private static IComparable NormalizeValue(IComparable value)
        {
            if ( value is Int32 || value is Int64 )
            {
                return Convert.ToDouble(value);
            }

            return value;   
        }

        private static IComparable GetValue(StacItem item, FunctionRef propertyRef)
        {
            // TBD
            throw new NotImplementedException();
        }

        private static IComparable GetValue(StacItem item, Models.Cql2.String @string)
        {
            return @string.Str;
        }

        private static IComparable GetValue(StacItem item, IScalarLiteral scalarLiteral)
        {
            if (scalarLiteral is InstantLiteral instantLiteral)
            {
                return instantLiteral.DateTime;
            }

            if (scalarLiteral is DateLiteral dateLiteral)
            {
                return dateLiteral.DateTime;
            }

            if (scalarLiteral is TimestampLiteral timestampLiteral)
            {
                return timestampLiteral.DateTime;
            }

            if (scalarLiteral is BoolExpression boolExpression)
            {
                return boolExpression.Bool;
            }

            if (scalarLiteral is Models.Cql2.String @string)
            {
                return @string.Str;
            }

            if (scalarLiteral is Number number)
            {
                return number.Num;
            }

            throw new NotSupportedException($"Literal {scalarLiteral.GetType()} is not supported");
        }

    }
}
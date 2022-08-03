using System;
using System.Collections.Generic;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class ComparisonPredicateConverter : JsonConverter
    {
        static Dictionary<Type, Type> possibleTypes =
            new Dictionary<Type, Type>(){
                { typeof(ComparisonPredicateOp), typeof(BinaryComparisonPredicate) },
                { typeof(IsLikePredicateOp), typeof(IsLikePredicate) },
                { typeof(IsBetweenPredicateOp), typeof(IsBetweenPredicate) },
                { typeof(IsNullPredicateOp), typeof(IsNullPredicate) },
                { typeof(IsInListPredicateOp), typeof(IsInListPredicate) },
                { typeof(SpatialPredicateOp), typeof(SpatialPredicate) },
                { typeof(TemporalPredicateOp), typeof(TemporalPredicate) },
            };

        public override bool CanConvert(Type objectType)
        {
            return typeof(ComparisonPredicate).IsAssignableFromType(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);

        }

        internal ComparisonPredicate ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (jo.ContainsKey("op"))
            {
                bool found = false;

                foreach (var type in possibleTypes)
                {
                    try
                    {
                        jo["op"].ToObject(type.Key, serializer);
                        found = true;
                    }
                    catch (JsonSerializationException) { }
                    if (found)
                        return (ComparisonPredicate)jo.ToObject(type.Value, serializer);
                }
            }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to ComparisonPredicate");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}
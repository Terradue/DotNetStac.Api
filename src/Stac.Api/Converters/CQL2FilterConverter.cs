using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class CQL2FilterConverter : JsonConverter
    {
        ComparisonPredicateConverter comparisonPredicateConverter = new ComparisonPredicateConverter();

        public CQL2FilterConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CQL2Expression);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public CQL2Expression ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // find the filter-lang. Default to cql2-text
            FilterLang? filter_Lang = FilterLang.Cql2Text;
            if (jo.ContainsKey("filter-lang"))
            {
                filter_Lang = StacAccessorsHelpers.LazyEnumParse(typeof(FilterLang), jo["filter-lang"].ToString()) as FilterLang?;
            }
            var booleanExpression = CreateFilter(jo["filter"] as JObject, filter_Lang);
            if (booleanExpression == null)
                return null;
            return new CQL2Expression(booleanExpression);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("filter-lang");
            writer.WriteValue("cql2-json");
            writer.WritePropertyName("filter");
            serializer.Serialize(writer, (value as CQL2Expression).Expression);
            writer.WriteEndObject();
        }

        public BooleanExpression CreateFilter(JObject filterParameter, FilterLang? filter_lang = FilterLang.Cql2Text )
        {
            if (filter_lang == null || filter_lang == FilterLang.Cql2Text)
            {
                return CreateCqlFilterFromText(filterParameter);
            }
            else if (filter_lang == FilterLang.Cql2Json)
            {
                return CreateCqlFilterFromJson(filterParameter);
            }
            else
            {
                throw new ArgumentException("Invalid filter_lang value");
            }
        }

        private BooleanExpression CreateCqlFilterFromJson(JObject filter)
        {
            var cql = filter.ToObject<BooleanExpression>();
            return cql;
        }

        private BooleanExpression CreateCqlFilterFromText(JObject filter)
        {
            throw new NotImplementedException();
        }

        
    }


}
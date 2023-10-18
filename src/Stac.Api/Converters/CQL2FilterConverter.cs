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
        private FilterLang? _filter_Lang;

        public CQL2FilterConverter()
        {
        }

        public CQL2FilterConverter(FilterLang value)
        {
            _filter_Lang = value;
        }

        public CQL2FilterConverter(FilterLang? filter_lang)
        {
            _filter_Lang = filter_lang;
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
            var booleanExpression = CreateFilter(_filter_Lang, jo);
            if (booleanExpression == null)
                return null;
            return new CQL2Expression(booleanExpression);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (value as CQL2Expression).Expression);
        }

        private BooleanExpression CreateFilter(FilterLang? filter_lang, JObject filterParameter)
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
            return null;
        }

        public enum FilterLang
        {

            [System.Runtime.Serialization.EnumMember(Value = @"cql2-text")]
            Cql2Text = 0,

            [System.Runtime.Serialization.EnumMember(Value = @"cql2-json")]
            Cql2Json = 1,

        }
    }


}
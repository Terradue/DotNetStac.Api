using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Converters;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Clients.Converters
{
    public class FilterSearchBodyConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FilterSearchBody);
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public FilterSearchBody ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            FilterLang? filter_lang = null;
            CQL2FilterConverter cql2FilterConverter = new CQL2FilterConverter();
            if (jo.ContainsKey("filter-lang"))
            {
                filter_lang = StacAccessorsHelpers.LazyEnumParse(typeof(FilterLang), jo["filter-lang"].ToString()) as FilterLang?;
                cql2FilterConverter = new CQL2FilterConverter(filter_lang != null ? Enum.Parse<CQL2FilterConverter.FilterLang>(filter_lang.ToString()) : null);
            }
            // Read additional properties
            var additionalProperties = jo.Properties().Where(p => p.Name != "filter" && p.Name != "filter-lang" && p.Name != "filter_crs").ToDictionary(p => p.Name, p => p.Value);
            
            return new FilterSearchBody(){
                FilterLang = filter_lang ?? FilterLang.Cql2Text,
                Filter =  cql2FilterConverter.ReadJObject(jo["filter"] as JObject, typeof(CQL2Filter), existingValue, serializer),
                FilterCrs = jo["filter_crs"]?.ToObject<Uri>(),
                AdditionalProperties = additionalProperties.Select(p => new KeyValuePair<string, object>(p.Key, p.Value.ToObject<object>())).ToDictionary(p => p.Key, p => p.Value)
            };
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


}
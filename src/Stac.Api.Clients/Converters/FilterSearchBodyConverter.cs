using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Clients.ItemSearch;
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
            // Read base object SearchBody
            SearchBody searchBody = serializer.Deserialize<SearchBody>(jo.CreateReader());

            // If there is no filter-lang nor filter, return the base object
            if (!jo.ContainsKey("filter-lang") && !jo.ContainsKey("filter"))
                return new FilterSearchBody(searchBody);

            CQL2FilterConverter cql2FilterConverter = new CQL2FilterConverter();
            CQL2Expression cQL2Expression = cql2FilterConverter.ReadJObject(jo, typeof(CQL2Expression), existingValue, serializer);
            
            // Read additional properties
            var additionalProperties = jo.Properties().Where(p => p.Name != "filter" && p.Name != "filter-lang" && p.Name != "filter_crs").ToDictionary(p => p.Name, p => p.Value);

            return new FilterSearchBody(searchBody){
                FilterLang = cQL2Expression.FilterLang,
                Filter =  cQL2Expression.Expression,
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
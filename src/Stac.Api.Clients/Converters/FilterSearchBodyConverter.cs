using System;
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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public FilterSearchBody ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (jo.ContainsKey("filter_lang"))
            {
                FilterLang? filter_lang = StacAccessorsHelpers.LazyEnumParse(typeof(FilterLang), jo["filter_lang"].ToString()) as FilterLang?;
                settings.Converters.Add(new CQL2FilterConverter(filter_lang != null ? Enum.Parse<CQL2FilterConverter.FilterLang>(filter_lang.ToString()) : null));
            }
            else
            {
                settings.Converters.Add(new CQL2FilterConverter());
            }
            return jo.ToObject<FilterSearchBody>(JsonSerializer.Create(settings));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}
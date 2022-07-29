﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Stac.Api.Tests
{
    /// <summary>
    ///     Assertions for json strings
    /// </summary>
    public static class JsonAssert
    {
        /// <summary>
        ///     Asserts that the json strings are equal.
        /// </summary>
        /// <remarks>
        ///     Parses each json string into a <see cref="JObject" />, sorts the properties of each
        ///     and then serializes each back to a json string for comparison.
        /// </remarks>
        /// <param name="expectJson">The expect json.</param>
        /// <param name="actualJson">The actual json.</param>
        public static void AreEqual(string expectJson, string actualJson)
        {
            Assert.Equal(
                JsonConvert.SerializeObject(JObject.Parse(expectJson).SortProperties(),
                    new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        Culture = CultureInfo.CreateSpecificCulture("en-US"),
                        Converters = GetConverters()
                    }),
                JsonConvert.SerializeObject(JObject.Parse(actualJson).SortProperties(),
                    new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        Culture = CultureInfo.CreateSpecificCulture("en-US"),
                        Converters = GetConverters()
                    })
            );
        }

        private static IList<JsonConverter> GetConverters()
        {
            return new List<JsonConverter>()
            {
            };
        }

        /// <summary>
        ///     Asserts that <paramref name="actualJson" /> contains <paramref name="expectedJson" />
        /// </summary>
        /// <param name="expectedJson">The expected json.</param>
        /// <param name="actualJson">The actual json.</param>
        public static void Contains(string expectedJson, string actualJson)
        {
            Assert.True(actualJson.Contains(expectedJson), string.Format("expected {0} to contain {1}", actualJson, expectedJson));
        }

        /// <summary>
        ///     Sorts the properties of a JObject
        /// </summary>
        /// <param name="jObject">The json object whhose properties to sort</param>
        /// <returns>A new instance of a <see cref="JObject" /> with sorted properties</returns>
        private static JObject SortProperties(this JObject jObject)
        {
            var result = new JObject();

            foreach (var property in jObject.Properties().OrderBy(p => p.Name))
            {
                if (property.Value is JObject value)
                {
                    value = value.SortProperties();
                    result.Add(property.Name, value);
                    continue;
                }


                if (property.Value is JArray avalues)
                {
                    if (avalues.Count == 0) continue;
                    avalues = avalues.SortProperties();
                    result.Add(property.Name, avalues);
                    continue;
                }

                result.Add(property.Name, property.Value);
            }

            return result;
        }

        private static JArray SortProperties(this JArray jArray)
        {
            var result = new JArray();

            foreach (var item in jArray)
            {
                if (item is JObject value)
                {
                    value = value.SortProperties();
                    result.Add(value);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }


    }
}

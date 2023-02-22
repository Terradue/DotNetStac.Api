using System;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Api.Interfaces;

namespace Stac.Api.Models.Cql2
{
    public abstract partial class BooleanExpression
    {
        private readonly JsonSerializerSettings _settings;

        public BooleanExpression()
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
        }
       
        /// <summary>
        /// Transforms the filter into a string representation
        /// </summary>
        /// <returns>A Json string representation</returns>
        public virtual string ToString()
        {
            return JsonConvert.SerializeObject(this, _settings);
        }

    }
}
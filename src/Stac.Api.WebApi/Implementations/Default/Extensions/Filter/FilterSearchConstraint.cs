using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.WebApi.Implementations.Default.Extensions.Filter
{
    public class FilterExtensionSearchConstraint : IRouteConstraint
    {
        private static readonly Regex _regex = new(
            @"^[1-9]*$",
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(100));

        public bool Match(
            HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            // In case, no route value is provided, we don't want to match.
            if (values is null || !values.Any())
            {
                return false;
            }

            if (!values.TryGetValue(routeKey, out var routeValue))
            {
                return false;
            }

            var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

            if (routeValueString is null)
            {
                return false;
            }

            return _regex.IsMatch(routeValueString);
        }
    }
}
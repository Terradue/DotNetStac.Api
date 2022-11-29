using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;
using Stars.Geometry.NTS;

namespace Stac.Api.WebApi.Implementations.Shared.Temporal
{
    public static class TemporalExtensions
    {
        public static bool Intersects(this Itenso.TimePeriod.ITimePeriod timeInterval, string stringInterval)
        {
            Itenso.TimePeriod.ITimeInterval timeIntervalFilter = stringInterval.ToTimeInterval();
            return timeInterval.IntersectsWith(timeIntervalFilter);
        }

        public static Itenso.TimePeriod.TimeInterval ToTimeInterval(this string stringInterval){
            string start = stringInterval;
            string end = stringInterval;
            if (stringInterval.Contains("/")){
                string[] split = stringInterval.Split("/");
                if (split.Length == 2){
                   start = split[0];
                   end = split[1];
                }
            }
            return new Itenso.TimePeriod.TimeInterval(DateTime.Parse(start, null, System.Globalization.DateTimeStyles.AssumeUniversal),
                                                      DateTime.Parse(end, null, System.Globalization.DateTimeStyles.AssumeUniversal));
        }
    }
}
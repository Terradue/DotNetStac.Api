using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;

namespace Stac.Api.Models
{
    public class PatchStacItem : StacItem
    {
        [JsonConstructor]
        public PatchStacItem() : base("patch", null)
        {
        }
    }
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Converters;

namespace Stac.Api.Models
{
    public partial class StacFeatureCollection : GeoJSON.Net.Feature.FeatureCollection, ILinksCollectionObject, IEnumerable<StacItem>
    {
        public StacFeatureCollection()
        {
            Features = new List<Feature>();
            this.Links = new ObservableCollection<StacLink>();
            (Links as ObservableCollection<StacLink>).CollectionChanged += LinksCollectionChanged;
            TimeStamp = DateTimeOffset.UtcNow;
        }

        public StacFeatureCollection(IEnumerable<StacItem> items): this()
        {
            Features.AddRange(items);
        }

        private void LinksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems.Cast<StacItem>())
                {
                    Features.Remove(oldItem);
                }
            }
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems.Cast<StacItem>())
                {
                    Features.Add(newItem);
                }
            }
        }

        public IEnumerator<StacItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        [JsonConverter(typeof(ListConverter<StacItem, Feature>))]
        public new List<Feature> Features { get; set; }

        public IEnumerable<StacItem> Items
        {
            get
            {
                return Features.Cast<StacItem>();
            }
        }

        /// <summary>
        /// A list of references to other documents.
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(CollectionConverter<StacLink>))]
        [JsonProperty("links", Order = 5)]
        public ICollection<StacLink> Links
        {
            get; private set;
        }

        [Newtonsoft.Json.JsonProperty("timeStamp", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TimeStamp { get; set; }

        [Newtonsoft.Json.JsonProperty("numberMatched", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int NumberMatched { get; set; }

        [Newtonsoft.Json.JsonProperty("numberReturned", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int NumberReturned => Features.Count;

        [JsonIgnore]
        public string Collection { get; set; }

    }
}
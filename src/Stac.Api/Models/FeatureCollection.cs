using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Converters;

namespace Stac.Api.Models
{
    public partial class StacFeatureCollection : GeoJSON.Net.Feature.FeatureCollection
    {
        public StacFeatureCollection()
        {
            Items = new ObservableCollection<StacItem>();
            (Items as ObservableCollection<StacItem>).CollectionChanged += ItemsCollectionChanged;
            this.Links = new ObservableCollection<StacLink>();
            (Links as ObservableCollection<StacLink>).CollectionChanged += LinksCollectionChanged;
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

        [JsonIgnore]
        public ObservableCollection<StacItem> Items { get; set; }

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
        public int NumberReturned { get; set; }

    }
}
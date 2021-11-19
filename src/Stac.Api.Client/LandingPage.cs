using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using System.Text.Json.Serialization;
using Semver;
using Stac;

namespace Stac.Api.Client
{
    [JsonConverter(typeof(LandingPageConverter))]
    public class LandingPage : IStacCatalog
    {
        private readonly IStacCatalog stacCatalog;
        private const string _conformsToFieldName = "conformsTo";
        private static object _lock;

        public LandingPage(IStacCatalog stacCatalog)
        {
            this.stacCatalog = stacCatalog;
            this.ConformanceClasses = new ObservableCollection<string>();
            (ConformanceClasses as ObservableCollection<StacLink>).CollectionChanged += ConformsToCollectionChanged;
        }

        private void ConformsToCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_lock)
            {
                var conformsTo = this.GetProperty<string[]>(_conformsToFieldName).ToList();
                if (e.OldItems != null)
                {
                    foreach (var oldConformance in e.OldItems.Cast<string>())
                    {
                        conformsTo.Remove(oldConformance);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var newConformance in e.NewItems.Cast<string>())
                    {
                        conformsTo.Add(newConformance);
                    }
                }
                this.SetProperty(_conformsToFieldName, conformsTo.ToArray());
            }
        }

        public string Id => stacCatalog.Id;

        public string Title => stacCatalog.Title;

        public SemVersion StacVersion => stacCatalog.StacVersion;

        public ICollection<StacLink> Links => stacCatalog.Links;

        public ContentType MediaType => stacCatalog.MediaType;

        public ICollection<string> StacExtensions => stacCatalog.StacExtensions;

        public IDictionary<string, object> Properties => stacCatalog.Properties;

        public IStacObject StacObjectContainer => stacCatalog.StacObjectContainer;

        public ICollection<string> ConformanceClasses
        {
            get; internal set;
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using Semver;
using Stac.Api.Converters;
using Stac.Converters;

namespace Stac.Api.Models
{
    [JsonConverter(typeof(LandingPageConverter))]
    public class LandingPage : IStacCatalog, IStacObject
    {
        private readonly StacCatalog stacCatalog;
        private const string _conformsToFieldName = "conformsTo";
        private static object _lock = new object();

        public LandingPage(string id, string description, IEnumerable<StacLink> links = null)
        {
            this.stacCatalog = new StacCatalog(id, description, links);
            this.ConformanceClasses = new ObservableCollection<string>();
            (ConformanceClasses as ObservableCollection<string>).CollectionChanged += ConformsToCollectionChanged;
        }

        public LandingPage(StacCatalog stacCatalog)
        {
            this.stacCatalog = stacCatalog;
            this.ConformanceClasses = new ObservableCollection<string>(this.GetProperty<Collection<string>>(_conformsToFieldName) ?? new Collection<string>());
            (ConformanceClasses as ObservableCollection<string>).CollectionChanged += ConformsToCollectionChanged;
        }

        private void ConformsToCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_lock)
            {
                Collection<string> conformsTo = this.GetProperty<Collection<string>>(_conformsToFieldName);
                if (conformsTo == null)
                {
                    conformsTo = new Collection<string>();
                }
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
                this.SetProperty(_conformsToFieldName, conformsTo);
            }
        }

        public string Id => stacCatalog.Id;

        public string Title
        {
            get
            {
                return stacCatalog.Title;
            }

            set
            {
                stacCatalog.Title = value;
            }
        }

        public string Description
        {
            get
            {
                return stacCatalog.Description;
            }

            set
            {
                stacCatalog.Description = value;
            }
        }

        public SemVersion StacVersion
        {
            get
            {
                return stacCatalog.StacVersion;
            }

            set
            {
                stacCatalog.StacVersion = value;
            }
        }

        public ICollection<StacLink> Links => stacCatalog.Links;

        public ContentType MediaType => stacCatalog.MediaType;

        public ICollection<string> StacExtensions => stacCatalog.StacExtensions;

        public IDictionary<string, object> Properties => stacCatalog.Properties;

        public IStacObject StacObjectContainer => stacCatalog.StacObjectContainer;

        [JsonConverter(typeof(CollectionConverter<string>))]
        public ICollection<string> ConformanceClasses
        {
            get; internal set;
        }

        public StacCatalog StacCatalog => stacCatalog;
    }
}
using System.Collections.Generic;

namespace Stac.Api.Interfaces
{
    public interface IStacPageable<T> : ILinksCollectionObject
    {
        ICollection<T> Items { get; }
    }
}
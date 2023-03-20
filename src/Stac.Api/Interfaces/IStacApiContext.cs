using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Interfaces
{
    public interface IStacApiContext: IStacPropertiesContainer
    {
        string Id { get; }
        Uri BaseUri { get; }
        LinkGenerator LinkGenerator { get; }
        HttpContext HttpContext { get; }
        IList<string> Collections { get; }
        void SetCollections(IList<string> collectionId);
        IList<ILinkValues> LinkValues { get; }
    }
}
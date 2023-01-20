using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Interfaces
{
    public interface IStacApiContext
    {
        Uri BaseUri { get; }
        LinkGenerator LinkGenerator { get; }
        HttpContext HttpContext { get; }
        IPaginationParameters PaginationParameters { get; }
        string Collection { get; }
        void SetCollection(string collectionId);
        void SetMatchedItemsCount(int length);
        void SetPaginationParameters(IPaginationParameters paginationParameters);
    }
}
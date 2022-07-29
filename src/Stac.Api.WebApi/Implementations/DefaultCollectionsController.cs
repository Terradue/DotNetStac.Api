using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultCollectionsController : DefaultBaseController, ICollectionsController
    {
        public DefaultCollectionsController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollections("examples/collections").FirstOrDefault(c => c.Id == collectionId);
            return collection == null ? new NotFoundResult() : (ActionResult<StacCollection>)collection;
        }

        public async Task<ActionResult<Collections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            return new Collections()
            {
                Collections1 = GetCollections("examples/collections").ToList()
            };
        }

        
    }
}
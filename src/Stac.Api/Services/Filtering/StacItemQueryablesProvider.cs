using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Schema;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Filtering
{
    public class StacItemQueryablesProvider : IQueryablesProvider<StacItem>
    {
        public StacItemQueryablesProvider(IOptionsMonitor<QueryablesOptions> queryablesOptions)
        {
            QueryablesOptions stacItemQueryablesOptions = queryablesOptions.Get(nameof(StacItem));
        }

        public JSchema GetQueryables()
        {
            throw new NotImplementedException();
        }
    }
}
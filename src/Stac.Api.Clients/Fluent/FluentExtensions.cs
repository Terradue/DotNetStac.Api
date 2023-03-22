using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Clients.ItemSearch;

namespace Stac.Api.Clients.Fluent
{
    public static class FluentExtensions
    {
        public static ItemSearch Search(this ItemSearchClient itemSearchClient){
            return new ItemSearch(itemSearchClient);
        }
    }
}
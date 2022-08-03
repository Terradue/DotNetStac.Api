using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Models.Fragments.Filter
{
    public abstract class FilterCql2Json : BooleanExpression, Fragments.Filter.IFilter
    {
        
    }
}
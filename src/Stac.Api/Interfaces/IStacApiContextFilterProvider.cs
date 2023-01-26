using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IStacApiContextFilterProvider
    {
        IEnumerable<IStacApiContextFilter> GetPostQueryFilters<T>() where T : IStacObject;
        IEnumerable<IStacApiContextFilter> GetPreQueryFilters<T>() where T : IStacObject;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace Stac.Api.Interfaces
{
    public interface IQueryablesProvider<T> where T : IStacObject
    {
        JSchema GetQueryables();
    }
}
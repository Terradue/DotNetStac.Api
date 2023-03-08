using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IStacResultObject<TResult> where TResult : IStacObject
    {
        int NumberMatched { get; }

        int NumberReturned { get; }
    }
}
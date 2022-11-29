using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IStacApiController
    {
        IReadOnlyCollection<string> GetConformanceClasses();
    }
}
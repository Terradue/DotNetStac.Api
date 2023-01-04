using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace Stac.Api.Interfaces
{
    public interface IStacQueryProvider : IQueryProvider
    {
        Expression GetFirstExpression();
        
        JSchema GetQueryables();
    }
}
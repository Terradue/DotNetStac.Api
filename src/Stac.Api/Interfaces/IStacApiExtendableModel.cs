using System.Collections.Generic;

namespace Stac.Api.Interfaces
{
    public interface IStacApiExtendableModel
    {
        IDictionary<string, object> AdditionalProperties { get; }
    }
}
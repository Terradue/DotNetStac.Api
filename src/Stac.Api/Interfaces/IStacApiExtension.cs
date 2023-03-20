using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    /// <summary>
    /// This interface represents an implementation of an extension to the STAC API
    /// </summary>
    public interface IStacApiExtension
    {
        /// <summary>
        /// Gets the Stac Extension identifier.
        /// </summary>
        string Identifier { get; } 
    }
}
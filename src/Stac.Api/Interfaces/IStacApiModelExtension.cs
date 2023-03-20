using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace Stac.Api.Interfaces
{
    /// <summary>
    /// Interface to represent a model extension
    /// </summary>
    public interface IStacApiModelExtension: IStacApiExtension
    {
        Type ExtendedModelType { get; }

        // method to add the extension to the model
        Task BindModelAsync(ModelBindingContext bindingContext);
    }
}
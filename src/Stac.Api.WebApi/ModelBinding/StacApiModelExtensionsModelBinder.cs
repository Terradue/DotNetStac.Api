using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using Stac.Api.Attributes;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.ModelBinding
{
    /// <summary>
    /// Model binder for additional properties (for extensions)
    /// </summary>
    internal class StacApiModelExtensionsModelBinder : IModelBinder
    {
        private readonly IModelBinder _defaultBinder;
        private readonly IEnumerable<Interfaces.IStacApiModelExtension> _extensions;

        public StacApiModelExtensionsModelBinder(IModelBinder defaultBinder, IEnumerable<IStacApiModelExtension> extensions)
        {
            _defaultBinder = defaultBinder;
            _extensions = extensions;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // this is an extension so we will call the default binder
            await _defaultBinder.BindModelAsync(bindingContext);

            // for each extension attribute, get the extension and try to bind the value
            foreach (var extension in _extensions.Where(e => e.ExtendedModelType.IsAssignableFrom(bindingContext.ModelType)))
            {
                try
                {
                    var additionalPropertiesForExt = extension.BindModelAsync(bindingContext);
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
                }

            }
        }
    }
}
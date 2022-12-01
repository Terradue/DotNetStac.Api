using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    public class CodeGenOptions
    {
        public string ApiVersion { get; set; }

        public CSharpClientGeneratorSettings CSharpClientGeneratorSettings { get; set; }

        public CSharpControllerGeneratorSettings CSharpControllerGeneratorSettings { get; set; }

        public IDictionary<string, OpenApiSpecification> Specifications { get; set; }

        public IEnumerable<UrlMapping> UrlMappings { get; set; }

        internal CSharpClientGeneratorSettings GenerateClientGeneratorSettings(string key)
        {
            CSharpClientGeneratorSettings settings = JsonConvert.DeserializeObject<CSharpClientGeneratorSettings>(JsonConvert.SerializeObject(CSharpClientGeneratorSettings));
            var spec = Specifications[key];
            settings.ClassName = spec.ClientClassName;
            settings.CSharpGeneratorSettings.Namespace = spec.ClientNamespace;
            settings.CSharpGeneratorSettings.ExcludedTypeNames = spec.ExcludedTypeNames.Concat(spec.ExcludedClientTypeNames).ToArray();
            settings.CSharpGeneratorSettings.TypeNameGenerator = new CustomTypeNameGenerator(spec);
            settings.AdditionalNamespaceUsages = settings.AdditionalNamespaceUsages.Concat(new string[] { spec.ControllerNamespace }).ToArray();
            return settings;
        }

        internal CSharpControllerGeneratorSettings GenerateControllerGeneratorSettings(string key)
        {
            CSharpControllerGeneratorSettings settings = CSharpControllerGeneratorSettings;
            var spec = Specifications[key];
            settings.ClassName = spec.ControllerClassName;
            settings.CSharpGeneratorSettings.Namespace = spec.ControllerNamespace;
            settings.CSharpGeneratorSettings.ExcludedTypeNames = spec.ExcludedTypeNames.ToArray();
            settings.CSharpGeneratorSettings.TypeNameGenerator = new CustomTypeNameGenerator(spec);
            return settings;
        }
    }

    public class UrlMapping
    {
        public string Url { get; set; }

        public string UrlChange { get; set; }
    }
}
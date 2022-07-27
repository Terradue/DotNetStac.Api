using System;
using System.Collections.Generic;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    public class CodeGenOptions
    {
        public CSharpClientGeneratorSettings CSharpClientGeneratorSettings { get;  set; }

        public CSharpControllerGeneratorSettings CSharpControllerGeneratorSettings { get;  set; }

        public IDictionary<string, OpenApiSpecification> Specifications { get; set; }

        internal CSharpClientGeneratorSettings GenerateClientGeneratorSettings(string key)
        {
            CSharpClientGeneratorSettings settings = CSharpClientGeneratorSettings;
            var spec = Specifications[key];
            settings.ClassName = spec.ClientClassName;
            settings.CSharpGeneratorSettings.Namespace = spec.ClientNamespace;
            settings.CSharpGeneratorSettings.ExcludedTypeNames = spec.ExcludedTypeNames;
            settings.CSharpGeneratorSettings.TypeNameGenerator = new CustomTypeNameGenerator(spec);
            return settings;
        }

        internal CSharpControllerGeneratorSettings GenerateControllerGeneratorSettings(string key)
        {
            CSharpControllerGeneratorSettings settings = CSharpControllerGeneratorSettings;
            var spec = Specifications[key];
            settings.ClassName = spec.ControllerClassName;
            settings.CSharpGeneratorSettings.Namespace = spec.ControllerNamespace;
            settings.CSharpGeneratorSettings.ExcludedTypeNames = spec.ExcludedTypeNames;
            settings.CSharpGeneratorSettings.TypeNameGenerator = new CustomTypeNameGenerator(spec);
            return settings;
        }
    }
}
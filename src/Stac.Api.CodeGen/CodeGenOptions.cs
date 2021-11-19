using System;
using System.Collections.Generic;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    public class CodeGenOptions
    {
        public CSharpClientGeneratorSettings CSharpClientGeneratorSettings { get;  set; }

        public IDictionary<string, OpenApiSpecification> Specifications { get; set; }

        internal CSharpClientGeneratorSettings GenerateSettings(string key)
        {
            CSharpClientGeneratorSettings settings = CSharpClientGeneratorSettings;
            var spec = Specifications[key];
            settings.CSharpGeneratorSettings.Namespace = spec.Namespace;
            settings.CSharpGeneratorSettings.ExcludedTypeNames = spec.ExcludedTypeNames;
            settings.CSharpGeneratorSettings.TypeNameGenerator = new CustomTypeNameGenerator(spec);
            return settings;
        }
    }
}
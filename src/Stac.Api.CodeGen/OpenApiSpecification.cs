using System;
using System.Collections.Generic;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    public class OpenApiSpecification
    {
        public string Url { get; set; }
        public string OutputFilePath { get; set; }
        public string[] ExcludedTypeNames { get; set; }
        public string Namespace { get; set; }
        public IDictionary<string, string> TypeNamesMapping { get; set; }
        public string ClassName { get; set; }
    }
}
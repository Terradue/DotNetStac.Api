using System;
using System.Collections.Generic;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    public class OpenApiSpecification
    {
        public string Url { get; set; }
        public string ClientOutputFilePath { get; set; }
        public string ControllerOutputFilePath { get; set; }
        public string[] ExcludedTypeNames { get; set; }
        public string ClientNamespace { get; set; }
        public string ControllerNamespace { get; set; }
        public IDictionary<string, string> TypeNamesMapping { get; set; }
        public string ClientClassName { get; set; }
        public string ControllerClassName { get; set; }
        public IEnumerable<string> ExcludedPaths { get; set; }
        public string OpenApiPath { get; set; }
        public string File { get; set; }
    }
}
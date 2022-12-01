using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    [JsonObject(MemberSerialization.OptOut)]
    public class OpenApiSpecification
    {
        public OpenApiSpecification()
        {
            ExcludedTypeNames = new Collection<string>();
            ExcludedSchemas = new Collection<string>();
            ExcludedOperations = new Collection<string>();
            TypeNamesMapping = new Dictionary<string, string>();
            ConformanceClasses = new Collection<string>();
        }

        public string Url { get; set; }
        public string ClientOutputFilePath { get; set; }
        public string ControllerOutputFilePath { get; set; }
        public ICollection<string> ExcludedTypeNames { get; set; }
        public string ClientNamespace { get; set; }
        public string ControllerNamespace { get; set; }
        public IDictionary<string, string> TypeNamesMapping { get; set; }
        public string ClientClassName { get; set; }
        public string ControllerClassName { get; set; }
        public ICollection<string> ExcludedOperations { get; set; }
        public string OpenApiPath { get; set; }
        public string File { get; set; }
        public ICollection<string> ExcludedSchemas { get; set; }
        public ICollection<string> ConformanceClasses { get; set; }
    }
}
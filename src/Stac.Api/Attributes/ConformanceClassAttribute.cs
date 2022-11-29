using System;

namespace Stac.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConformanceClassAttribute : Attribute
    {
        public ConformanceClassAttribute(string conformanceClass)
        {
            ConformanceClass = conformanceClass;
        }

        public string ConformanceClass { get; }
    }
}
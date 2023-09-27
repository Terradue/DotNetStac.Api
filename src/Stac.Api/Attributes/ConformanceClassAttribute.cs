

using System;

namespace Stac.Api.Attributes
{
    /// <summary>
    /// This attribute is used to define a conformance class for a given controller.
    /// </summary>
    /// <remarks>
    /// This attribute is used to define a conformance class for a given controller. The conformance class will be
    /// included in the conformance classes list of the landing page.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConformanceClassAttribute : Attribute
    {
        /// <summary>
        /// Conformance class attribute
        /// </summary>
        /// <param name="conformanceClass">Conformance class</param>
        public ConformanceClassAttribute(string conformanceClass)
        {
            ConformanceClass = conformanceClass;
        }

        /// <summary>
        /// The conformance class of the server.
        /// </summary>
        /// <remarks>
        /// The conformance class of the server is a url.
        /// </remarks>
        public string ConformanceClass { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is OGC API compliant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is OGC API; otherwise, <c>false</c>.
        /// </value>
        public bool IsOgcApi { get; set; } = false;

    }
}
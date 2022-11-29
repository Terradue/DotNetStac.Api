using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    internal class TestCatalogsProvider : TestBase, IEnumerable<object[]>
    {
        public TestCatalogsProvider() : base(null)
        {
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var subdir in Directory.GetDirectories(GetTestCatalogsRootPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false }))
            {
                yield return new object[] { Path.GetFileName(subdir), subdir };
            }
        }
    }
}
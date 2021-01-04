using Kuli.Importing;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Kuli.Tests
{
    public class RawFragmentImporterTests
    {
        private const string FencesOnly = @"
---
---
";

        private const string FencesAndValues = @"
---
foo: bar
---
";

        private const string MarkdownOnly = @"
Hello, World!
";

        private const string Mixed = @"
---
foo: bar
---
Hello, World!
";

        private RawFragmentImporterService _importerService;

        [SetUp]
        public void SetUp()
        {
            _importerService = new RawFragmentImporterService(NullLogger<RawFragmentImporterService>.Instance);
        }

        [TestCase(FencesOnly, "\r\n", "\r\n")]
        [TestCase(FencesAndValues, "\r\nfoo: bar\r\n", "\r\n")]
        [TestCase(MarkdownOnly, "", "\r\nHello, World!\r\n")]
        [TestCase(Mixed, "\r\nfoo: bar\r\n", "\r\nHello, World!\r\n")]
        [TestCase("", "", "")]
        [TestCase("Hello, World!", "", "Hello, World!")]
        public void RawFragmentImportTests(string input, string expectedFrontMatter, string expectedMarkdown)
        {
            var element = _importerService.Import("", input);
            Assert.AreEqual(expectedFrontMatter, element.FrontMatter);
            Assert.AreEqual(expectedMarkdown, element.PageMarkdown);
        }

        [TestCase("---")]
        public void RawFragmentImportInvalidTests(string input)
        {
            Assert.Throws<ImportException>(() => _importerService.Import("", input));
        }
    }
}
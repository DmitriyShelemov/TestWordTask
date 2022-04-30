using Aspose.Words;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using WordParsing.Logic;

namespace WordParsing.Tests
{
    /// <summary>
    /// Test coverage sample
    /// </summary>
    public class WordParsingServiceTests
    {
        private const string HeadText = "HeadText";
        private const string NoteText = "NoteText";
        private const string RegularText = "RegularText";

        private WordParsingService _service;
        private Mock<IAsposeWrapper> _asposeWrapper;

        [SetUp]
        public void Setup()
        {
            _asposeWrapper = new Mock<IAsposeWrapper>();
            _service = new WordParsingService(_asposeWrapper.Object);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetNodesByFilePathTest_Null(string path)
        {
            // act
            Action act = () => _service.GetNodes(path);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetNodesByStreamTest_Null()
        {
            // act
            Action act = () => _service.GetNodes(null as Stream);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase(true, true, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void GetNodesByFilePathTest(bool hasHeaders, bool hasNotes, bool hasTexts)
        {
            // arrange
            var doc = new Document();
            _asposeWrapper.Setup(x => x.GetDocument(It.IsAny<string>())).Returns(doc);
            var headers = !hasHeaders ? Array.Empty<Node>() : new[] { new FakeNode(NodeType.HeaderFooter, HeadText) };
            _asposeWrapper.Setup(x => x.GetHeadersAndFooters(doc)).Returns(headers);
            var notes = !hasNotes ? Array.Empty<Node>() : new[] { new FakeNode(NodeType.Footnote, NoteText) };
            _asposeWrapper.Setup(x => x.GetNotes(doc)).Returns(notes);
            var regular = !hasTexts ? Array.Empty<Node>() : new[] { new FakeNode(NodeType.Paragraph, RegularText) };
            _asposeWrapper.Setup(x => x.GetTexts(doc)).Returns(regular);

            // act
            var nodes = _service.GetNodes("somepath");

            // assert
            Assert.NotNull(nodes);

            if (hasHeaders || hasNotes || hasTexts)
            {
                Assert.AreEqual(1, nodes.Count());
                Assert.AreEqual(hasHeaders ? NodeType.HeaderFooter : hasNotes ? NodeType.Footnote : NodeType.Paragraph, nodes.First().NodeType);
            }
            else
            {
                Assert.True(!nodes.Any());
            }
        }
    }
}
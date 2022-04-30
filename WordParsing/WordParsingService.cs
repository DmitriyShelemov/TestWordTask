using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordParsing.Logic
{
    public class WordParsingService : IWordParsingService
    {
        private readonly IAsposeWrapper _wrapper;

        public WordParsingService(IAsposeWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public bool ValidateFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            return ValidateFileName(filePath);
        }

        public bool ValidateFileName(string filePath)
        {
            var extension = Path.GetExtension(filePath?.ToLower() ?? string.Empty);
            return extension == ".doc" || extension == ".docx";
        }

        public IEnumerable<Aspose.Words.Node> GetNodes(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var doc = _wrapper.GetDocument(filePath);
            return FindNodes(doc);
        }

        public IEnumerable<Aspose.Words.Node> GetNodes(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var doc = _wrapper.GetDocument(stream);
            return FindNodes(doc);
        }

        public IList<string> GetTexts(IEnumerable<Aspose.Words.Node> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            const string splitter = "---------------------------------";

            if (nodes.Any())
                return nodes
                    .SelectMany(x => x.GetText().Split("\r").Append(splitter))
                    .Distinct()
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

            return Array.Empty<string>();
        }

        public string GetSingleText(IEnumerable<Aspose.Words.Node> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (nodes.Any())
            {
                var texts = nodes
                    .Select(x => x.GetText())
                    .Distinct()
                    .Where(x => !string.IsNullOrWhiteSpace(x));

                return string.Join(Environment.NewLine + Environment.NewLine, texts);
            }

            return null;
        }

        private IEnumerable<Aspose.Words.Node> FindNodes(Aspose.Words.Document doc)
        {
            var headersAndFooters = _wrapper.GetHeadersAndFooters(doc);
            if (headersAndFooters.Any(x => !string.IsNullOrWhiteSpace(x.GetText())))
                return headersAndFooters;

            var notes = _wrapper.GetNotes(doc);
            if (notes.Any(x => !string.IsNullOrWhiteSpace(x.GetText())))
                return notes;

            var paragraphs = _wrapper.GetTexts(doc);

            if (paragraphs.Any(x => !string.IsNullOrWhiteSpace(x.GetText())))
                return paragraphs;

            return Array.Empty<Aspose.Words.Node>();
        }
    }
}

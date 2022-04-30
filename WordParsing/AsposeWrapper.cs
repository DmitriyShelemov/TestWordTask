using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordParsing.Logic
{
    public class AsposeWrapper : IAsposeWrapper
    {
        private const string TrialText = "Created with an evaluation copy of Aspose.Words. To discover the full versions of our APIs please visit: https://products.aspose.com/words/";

        public Aspose.Words.Document GetDocument(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return new Aspose.Words.Document(filePath);
        }

        public Aspose.Words.Document GetDocument(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Position = 0;
            return new Aspose.Words.Document(stream);
        }

        public IEnumerable<Aspose.Words.Node> GetHeadersAndFooters(Aspose.Words.Document doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            return doc.GetChildNodes(Aspose.Words.NodeType.HeaderFooter, true)
                .Where(x => x is HeaderFooter 
                && (((HeaderFooter)x).HeaderFooterType == HeaderFooterType.HeaderPrimary
                    || ((HeaderFooter)x).HeaderFooterType == HeaderFooterType.FooterPrimary)
                && x.GetText()?.Trim() != TrialText);
        }

        public IEnumerable<Aspose.Words.Node> GetNotes(Aspose.Words.Document doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            return doc.GetChildNodes(Aspose.Words.NodeType.Footnote, true);
        }

        public IEnumerable<Aspose.Words.Node> GetTexts(Aspose.Words.Document doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            return doc.Sections
                .ToArray()
                .Select(x => x.GetChild(Aspose.Words.NodeType.Paragraph, 0, true));
        }
    }
}

using Aspose.Words;
using System.Collections.Generic;
using System.IO;

namespace WordParsing.Logic
{
    public interface IAsposeWrapper
    {
        Document GetDocument(Stream stream);
        Document GetDocument(string filePath);
        IEnumerable<Node> GetHeadersAndFooters(Document doc);
        IEnumerable<Node> GetNotes(Document doc);
        IEnumerable<Node> GetTexts(Document doc);
    }
}
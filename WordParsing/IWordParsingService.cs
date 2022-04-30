using Aspose.Words;
using System.Collections.Generic;
using System.IO;

namespace WordParsing.Logic
{
    public interface IWordParsingService
    {
        IEnumerable<Node> GetNodes(string filePath);

        IEnumerable<Aspose.Words.Node> GetNodes(Stream stream);

        string GetSingleText(IEnumerable<Node> nodes);

        IList<string> GetTexts(IEnumerable<Node> nodes);

        bool ValidateFile(string filePath);

        bool ValidateFileName(string filePath);
    }
}
using Aspose.Words;

namespace WordParsing.Tests
{
    internal class FakeNode : Node
    {
        private NodeType _nodeType;
        private string _text;

        public FakeNode(NodeType nodeType, string text)
        {
            _nodeType = nodeType;
            _text = text;
        }

        public override NodeType NodeType => _nodeType;

        public override string GetText()
        {
            return _text;
        }

        public override bool Accept(DocumentVisitor visitor)
        {
            return true;
        }
    }
}

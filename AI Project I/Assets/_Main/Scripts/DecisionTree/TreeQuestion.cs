using System;

namespace Game.DecisionTree
{
    public class TreeQuestion : ITreeNode
    {
        private Func<bool> _question;
        private ITreeNode _tNode;
        private ITreeNode _fNode;

        public TreeQuestion(Func<bool> question, ITreeNode tNode, ITreeNode fNode)
        {
            _question = question;
            _tNode = tNode;
            _fNode = fNode;
        }

        public void Execute()
        {
            if (_question())
            {
                _tNode.Execute();
            }
            else
            {
                _fNode.Execute();
            }
        }
    }
}
using System;

namespace Game.DecisionTree
{
    public class TreeAction : ITreeNode
    {
        private Action _action;

        public TreeAction(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            if (_action != null)
                _action();
        }
    }
}
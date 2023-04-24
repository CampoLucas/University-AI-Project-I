using System;

namespace Game.DecisionTree
{
    /// <summary>
    /// This class represents a node in a decision tree that performs a specific action when executed.
    /// </summary>
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
            {
                _action();
            }
        }
        
        public void Dispose()
        {
            _action = null;
            Logging.LogDestroy("Action Nullified");
        }
    }
}
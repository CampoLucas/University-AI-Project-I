using System;

namespace Game.DecisionTree
{
    /// <summary>
    /// The basic structure of a decision tree node. 
    /// </summary>
    public interface ITreeNode : IDisposable
    {
        /// <summary>
        /// Method that executes the tree logic
        /// </summary>
        void Execute();
    }
}
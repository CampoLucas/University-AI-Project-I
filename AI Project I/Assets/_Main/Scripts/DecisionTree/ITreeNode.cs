using System;

namespace Game.DecisionTree
{
    public interface ITreeNode : IDisposable
    {
        void Execute();
    }
}
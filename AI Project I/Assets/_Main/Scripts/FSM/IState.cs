using System;

namespace Game.FSM
{
    public interface IState<T> : IDisposable
    {
        void Awake();
        void Execute();
        void Sleep();
        IState<T> GetTransition(T input);
        void AddTransition(T input, IState<T> state);
        void RemoveTransition(IState<T> state);
        void RemoveTransition(T input);
    }
}

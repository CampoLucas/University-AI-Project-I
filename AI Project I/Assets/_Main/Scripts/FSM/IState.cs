using System;
using System.Collections.Generic;

namespace Game.FSM
{
    /// <summary>
    /// A state in a Finite State Machine for a generic type T.
    /// </summary>
    public interface IState<T> : IDisposable
    {
        /// <summary>
        /// A method that is called when the state is activated.
        /// </summary>
        void Awake();
        /// <summary>
        /// A method that is called every frame while the state is active.
        /// </summary>
        void Execute();
        /// <summary>
        /// A method that is called when the state is deactivated.
        /// </summary>
        void Sleep();
        /// <summary>
        /// A method that returns the next state based on the provided input parameter.
        /// </summary>
        IState<T> GetTransition(T input);
        /// <summary>
        /// A method that adds a new transition from the current state to the provided state based on the provided input parameter.
        /// </summary>
        void AddTransition(T input, IState<T> state);
        /// <summary>
        /// A method that adds a dictionary to the transitions dictionary.
        /// The keys are of type T and represent the input that triggers the transition, and the values are of type IState that represent the state that the transition leads to.
        /// </summary>
        void AddTransition(Dictionary<T, IState<T>> transitions);
        /// <summary>
        /// A method that removes a transition from the current state to the provided state.
        /// </summary>
        void RemoveTransition(IState<T> state);
        /// <summary>
        /// A method that removes a transition based on the provided input parameter.
        /// </summary>
        /// <param name="input"></param>
        void RemoveTransition(T input);
    }
}

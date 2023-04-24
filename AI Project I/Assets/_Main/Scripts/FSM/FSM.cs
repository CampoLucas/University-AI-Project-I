using System;

namespace Game.FSM
{
    /// <summary>
    /// A generic class that implements a Finite State Machine for a generic type T.
    /// </summary>
    public class FSM<T> : IDisposable
    {
        private IState<T> _current;
        
        /// <summary>
        /// A default constructor that creates a new FSM object.
        /// </summary>
        public FSM() {}

        /// <summary>
        /// A constructor that creates a new FSM object and sets an initial state using the SetInit method.
        /// </summary>
        public FSM(IState<T> init)
        {
            SetInit(init);
        }

        /// <summary>
        /// A method that sets the initial state of the FSM to the provided state.
        /// </summary>
        /// <param name="init">the provided state</param>
        public void SetInit(IState<T> init)
        {
            _current = init;
            _current.Awake();
        }

        /// <summary>
        /// A method that updates the current state of the FSM by calling its Execute method.
        /// </summary>
        public void OnUpdate()
        {
            if (_current != null)
                _current.Execute();
        }

        /// <summary>
        /// A method that performs a transition from the current state to a new state based on the provided input parameter.
        /// The new state is obtained by calling the GetTransition method on the current state.
        /// If the new state is not null, the current state is put to sleep, the new state becomes the current state, and its Awake method is called.
        /// </summary>
        /// <param name="input"></param>
        public void Transitions(T input)
        {
            var newState = _current.GetTransition(input);
            if (newState == null) return;
            _current.Sleep();
            _current = newState;
            _current.Awake();
        }

        /// <summary>
        /// A method that disposes of the current state and nullifies it.
        /// </summary>
        public void Dispose()
        {
            _current.Dispose();
            Logging.LogDestroy("State Disposed");
            _current = null;
            Logging.LogDestroy("State Nullified");
        }
    }
}

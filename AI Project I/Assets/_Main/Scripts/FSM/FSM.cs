using System;

namespace Game.FSM
{
    public class FSM<T> : IDisposable
    {
        private IState<T> _current;
        
        public FSM() {}

        public FSM(IState<T> init)
        {
            SetInit(init);
        }

        public void SetInit(IState<T> init)
        {
            _current = init;
            _current.Awake();
        }

        public void OnUpdate()
        {
            if (_current != null)
                _current.Execute();
        }

        public void Transitions(T input)
        {
            var newState = _current.GetTransition(input);
            if (newState == null) return;
            _current.Sleep();
            _current = newState;
            _current.Awake();
        }

        public void Dispose()
        {
            _current.Dispose();
            Logging.LogDestroy("State Disposed");
            _current = null;
            Logging.LogDestroy("State Nullified");
        }
    }
}

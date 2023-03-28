using System.Collections.Generic;
using System.Linq;

namespace Game.FSM
{
    public class State<T> : IState<T>
    {
        private Dictionary<T, IState<T>> _transitions = new();

        public virtual void Awake()
        {
            
        }

        public virtual void Execute()
        {
            
        }

        public virtual void Sleep()
        {
            
        }

        public IState<T> GetTransition(T input) => _transitions.ContainsKey(input) ? _transitions[input] : null;
        public void AddTransition(T input, IState<T> state) => _transitions[input] = state;

        public void AddTransition(Dictionary<T, IState<T>> transitions) => _transitions =
            _transitions.Union(transitions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        public void RemoveTransition(IState<T> state)
        {
            foreach(var item in _transitions)
            {
                if (item.Value != state) continue;
                _transitions.Remove(item.Key);
                break;
            }
        }

        public void RemoveTransition(T input)
        {
            if (_transitions.ContainsKey(input))
                _transitions.Remove(input);
        }
    }
}

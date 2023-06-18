using Game.FSM;

namespace Game.Player.States
{
    /// <summary>
    /// A state class that is inherited by all the player state classes.
    /// </summary>
    [System.Serializable]
    public class PlayerStateBase<T> : State<T>
    {
        protected PlayerModel Model;
        protected PlayerView View;
        protected PlayerInputHandler Inputs;
        protected FSM<T> Fsm;
        private T _inDamage;
        private T _inDead;

        /// <summary>
        /// A default constructor that creates a new PlayerStateBase object.
        /// </summary>
        public PlayerStateBase() {}
        /// <summary>
        /// A constructor that creates a new FSM object and takes the input for damage and death.
        /// </summary>
        public PlayerStateBase(T inDamage, T inDead)
        {
            _inDamage = inDamage;
            _inDead = inDead;
        }

        /// <summary>
        /// A method that initializes the provided parameters.
        /// </summary>
        public void Init(PlayerModel model, PlayerView view, PlayerInputHandler inputs, FSM<T> fsm)
        {
            Model = model;
            View = view;
            Fsm = fsm;
            Inputs = inputs;
        }

        /// <summary>
        /// When it awakes it subscribes to the damage and dead events because any state has to be able to transition to those states.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage += OnDamageHandler;
                Model.Damageable.OnDie += OnDeadHandler;
            }
        }

        /// <summary>
        /// When it changes states it unsubscribes to all the events.
        /// </summary>
        public override void Sleep()
        {
            base.Sleep();
            UnsubscribeAll();
        }
        
        /// <summary>
        /// A method that unsubscribes to all events
        /// </summary>
        public void UnsubscribeAll()
        {
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDeadHandler;
            }
        }

        private void OnDamageHandler() => Fsm.Transitions(_inDamage);
        private void OnDeadHandler() => Fsm.Transitions(_inDead);

        /// <summary>
        /// A method that nullifies all references and unsubscribes from all events
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            UnsubscribeAll();
            Model = null;
            View = null;
            Inputs = null;
            Fsm = null;
        }
    }
}
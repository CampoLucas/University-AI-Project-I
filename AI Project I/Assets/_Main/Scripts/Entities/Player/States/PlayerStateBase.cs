using Game.Entities;
using Game.FSM;

namespace Game.Player.States
{
    [System.Serializable]
    public class PlayerStateBase<T> : State<T>
    {
        protected PlayerModel Model;
        protected PlayerView View;
        protected PlayerInputHandler Inputs;
        protected FSM<T> Fsm;
        private T _inDamage;
        private T _inDead;

        public PlayerStateBase() {}
        public PlayerStateBase(T inDamage, T inDead)
        {
            _inDamage = inDamage;
            _inDead = inDead;
        }

        public void Init(PlayerModel model, PlayerView view, PlayerInputHandler inputs, FSM<T> fsm)
        {
            Model = model;
            View = view;
            Fsm = fsm;
            Inputs = inputs;
        }

        public override void Awake()
        {
            base.Awake();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage += OnDamageHandler;
                Model.Damageable.OnDie += OnDeadHandler;
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDeadHandler;
            }
        }

        private void OnDamageHandler() => Fsm.Transitions(_inDamage);
        private void OnDeadHandler() => Fsm.Transitions(_inDead);
    }
}
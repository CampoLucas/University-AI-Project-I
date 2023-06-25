using Game.DecisionTree;
using Game.FSM;
using UnityEngine;

namespace Game.Entities.Slime.States
{
    public abstract class SlimeStateBase<T> : State<T>
    {
        protected SlimeModel Model { get; private set; }
        protected SlimeController Controller { get; private set; }
        protected FSM<T> Fsm { get; private set; }
        protected ITreeNode Tree { get; private set; }

        public SlimeStateBase()
        {
            
        }

        public void Init(SlimeModel model,SlimeController controller, FSM<T> fsm, ITreeNode tree)
        {
            Model = model;
            Controller = controller;
            Fsm = fsm;
            Tree = tree;
        }

        public override void Awake()
        {
            base.Awake();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage += OnDamageHandler;
                Model.Damageable.OnDie += OnDieHandler;
            }
            
        }

        public override void Sleep()
        {
            base.Sleep();
            UnsubscribeAll();
        }

        protected void UnsubscribeAll()
        {
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDieHandler;
            }
        }

        private void OnDamageHandler()
        {
            Tree.Execute();
        }

        private void OnDieHandler()
        {
            Tree.Execute();
        }
    }
}
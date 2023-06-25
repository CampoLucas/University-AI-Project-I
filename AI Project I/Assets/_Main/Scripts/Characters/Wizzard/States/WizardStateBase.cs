using Game.DecisionTree;
using Game.FSM;
using UnityEngine;

namespace Game.Enemies.States
{
    /// <summary>
    /// A state class that is inherited by all the enemy state classes.
    /// </summary>
    public class WizardStateBase<T> : State<T>
    {

        protected WizardModel Model;
        protected EnemyView View;
        protected WizardController Controller;
        protected ITreeNode Tree;

        /// <summary>
        /// A method that initializes the provided parameters.
        /// </summary>
        public void Init(WizardModel model, EnemyView view, WizardController controller, ITreeNode tree)
        {
            LoggingTwo.Log("Fuck me this! Model is null", () => model == null);
            
            Model = model;
            View = view;
            Controller = controller;
            Tree = tree;
        }

        /// <summary>
        /// When it awakes it subscribes to the damage and dead events because any state has to be able to transition to those states.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            
            LoggingTwo.Log("Model = = null", ()=> Model == null);
            LoggingTwo.Log("damageable = = null", ()=> Model.Damageable == null);
            if (Model && Model.Damageable != null)
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
            if (Model && Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDeadHandler;
            }
        }

        private void OnDamageHandler() => Tree.Execute();
        private void OnDeadHandler() => Tree.Execute();

        /// <summary>
        /// A method that nullifies all references and unsubscribes from all events
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            UnsubscribeAll();
            Model = null;
            View = null;
            Controller = null;
            Tree = null;
        }
    }
}
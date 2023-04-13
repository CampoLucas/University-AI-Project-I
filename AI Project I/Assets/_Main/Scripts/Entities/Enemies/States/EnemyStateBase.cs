using Game.DecisionTree;
using Game.FSM;

namespace Game.Enemies.States
{
    public class EnemyStateBase<T> : State<T>
    {

        protected EnemyModel Model;
        protected EnemyView View;
        protected EnemyController Controller;
        protected ITreeNode Tree;

        public void Init(EnemyModel model, EnemyView view, EnemyController controller, ITreeNode tree)
        {
            Model = model;
            View = view;
            Controller = controller;
            Tree = tree;
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

        private void OnDamageHandler() => Tree.Execute();
        private void OnDeadHandler() => Tree.Execute();

        public override void Dispose()
        {
            base.Dispose();
            Model = null;
            View = null;
            Controller = null;
            Tree = null;
        }
    }
}
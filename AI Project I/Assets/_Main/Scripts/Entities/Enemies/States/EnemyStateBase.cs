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
    }
}
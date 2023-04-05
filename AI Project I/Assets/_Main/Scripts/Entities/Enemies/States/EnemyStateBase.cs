using Game.FSM;

namespace Game.Enemies.States
{
    public class EnemyStateBase<T> : State<T>
    {

        protected EnemyModel Model;
        protected EnemyView View;
        protected EnemyController Controller;
        protected FSM<T> Fsm;

        public void Init(EnemyModel model, EnemyView view, EnemyController controller, FSM<T> fsm)
        {
            Model = model;
            View = view;
            Fsm = fsm;
        }
    }
}
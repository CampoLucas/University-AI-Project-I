using Game.FSM;

namespace Game.Entities.Slime.States
{
    public abstract class SlimeStateBase<T> : State<T>
    {
        protected SlimeModel Model { get; private set; }
        protected SlimeController Controller { get; private set; }
        protected FSM<T> Fsm { get; private set; }

        public SlimeStateBase()
        {
            
        }

        public void Init(SlimeModel model,SlimeController controller, FSM<T> fsm)
        {
            Model = model;
            Controller = controller;
            Fsm = fsm;
        }
    }
}
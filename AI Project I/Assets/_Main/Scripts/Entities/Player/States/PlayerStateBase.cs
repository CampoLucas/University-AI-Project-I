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

        public void Init(PlayerModel model, PlayerView view, PlayerInputHandler inputs, FSM<T> fsm)
        {
            Model = model;
            View = view;
            Fsm = fsm;
            Inputs = inputs;
        }
    }
}
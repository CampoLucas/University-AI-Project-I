using Game.FSM;
using Game.InputActions;
using UnityEngine;

namespace Game.Player.States
{
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
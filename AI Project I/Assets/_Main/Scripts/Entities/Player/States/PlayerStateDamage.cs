using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDamage<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDead;

        public PlayerStateDamage(in T inIdle, in T inMoving)// inDead
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Awake()
        {
            base.Awake();
            if (!Model.IsAlive())
            {
                Fsm.Transitions(_inDead);
            }
            View.PlayTargetAnimation(Model.GetData().HitAnimation.name);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetCurrentTimer() > 0)
            {
                Model.RunTimer();
            }
            else
            {
                if (Inputs.MoveDir != Vector3.zero)
                {
                    Fsm.Transitions(_inMoving);
                }
                else
                {
                    Fsm.Transitions(_inIdle);
                }
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}
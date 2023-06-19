using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDamage<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateDamage(in T inIdle, in T inMoving, in T inDamage, in T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().HitAnimation.name);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetTimerComplete())
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
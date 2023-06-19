using Game.Entities;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateHeavyAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateHeavyAttackOne(T inIdle, T inMoving, T inDamage, T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Awake()
        {
            base.Awake();
            Model.HeavyAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().HeavyAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().HeavyAttack01.Duration;
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
            Model.CancelHeavyAttack();
        }
    }
}
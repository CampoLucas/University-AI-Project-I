using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateLightAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateLightAttackOne(T inIdle, T inMoving, T inDamage, T inDead): base(inDamage, inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Awake()
        {
            base.Awake();
            Model.LightAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().LightAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().LightAttack01.Duration;
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
            Model.CancelLightAttack();
        }
    }
}
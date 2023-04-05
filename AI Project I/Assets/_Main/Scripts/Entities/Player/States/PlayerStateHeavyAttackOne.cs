using Game.Entities;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateHeavyAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDamage;

        public PlayerStateHeavyAttackOne(T inIdle, T inMoving, T inDamage)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inDamage = inDamage;
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
            if (Model.HasTakenDamage())
            {
                Fsm.Transitions(_inDamage);
            }

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
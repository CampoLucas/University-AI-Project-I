using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateIdle<T> : PlayerStateBase<T>
    {
        private readonly T _inMoving;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;

        public PlayerStateIdle(in T inMoving, in T inLightAttack, in T inHeavyAttack, in T inDamage, in T inDead): base(inDamage, inDead)
        {
            _inMoving = inMoving;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
        }

        public override void Execute()
        {
            base.Execute();
            if (Inputs.MoveDir != Vector3.zero)
            {
                Fsm.Transitions(_inMoving);
            }

            if (Inputs.FlagLightAttack)
            {
                Fsm.Transitions(_inLightAttack);
            }

            if (Inputs.FlagHeavyAttack)
            {
                Fsm.Transitions(_inHeavyAttack);
            }
            
            View.UpdateMovementValues(Inputs.MoveAmount);
            
        }
    }
}
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateIdle<T> : PlayerStateBase<T>
    {
        private readonly T _inMoving;
        private readonly T _inAttack;
        private readonly T _inDamage;

        public PlayerStateIdle(in T inMoving, in T inAttack, in T inDamage)
        {
            _inMoving = inMoving;
            _inAttack = inAttack;
            _inDamage = inDamage;
        }

        public override void Awake()
        {
            base.Awake();
            Debug.Log("Idle");
        }

        public override void Execute()
        {
            base.Execute();
            if (Inputs.MoveDir != Vector3.zero)
            {
                Fsm.Transitions(_inMoving);
            }

            if (Inputs.FlagAttack)
            {
                Fsm.Transitions(_inAttack);
            }
            
            View.UpdateMovementValues(Inputs.MoveAmount);
        }
    }
}
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateLightAttackTwo<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDamage;

        public PlayerStateLightAttackTwo(T inIdle, T inMoving, T inDamage)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inDamage = inDamage;
        }

        public override void Awake()
        {
            base.Awake();
            Model.LightAttack(View);
        }

        public override void Execute()
        {
            base.Execute();
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
}
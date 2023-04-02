using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateLightAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inLightAttackTwo;
        private readonly T _inDamage;

        public PlayerStateLightAttackOne(T inIdle, T inMoving, T inLightAttackTwo, T inDamage)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inLightAttackTwo = inLightAttackTwo;
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
            if (View.IsInteracting()) return;
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
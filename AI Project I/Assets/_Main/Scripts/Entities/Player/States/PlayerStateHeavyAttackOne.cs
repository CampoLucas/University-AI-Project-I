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
            Model.HeavyAttack(View);
#if UNITY_EDITOR
            Debug.Log("Heavy");
#endif
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
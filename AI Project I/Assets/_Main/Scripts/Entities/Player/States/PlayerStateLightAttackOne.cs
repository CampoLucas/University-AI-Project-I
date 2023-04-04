using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateLightAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDamage;
        private float _timeElapsed;

        public PlayerStateLightAttackOne(T inIdle, T inMoving, T inDamage)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inDamage = inDamage;
        }

        public override void Awake()
        {
            base.Awake();
            Model.LightAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().LightAttack01.EventName);
            _timeElapsed = 0;
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.TakesDamage())
            {
                Fsm.Transitions(_inDamage);
            }
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= Model.CurrentWeapon().GetData().LightAttack01.Duration)
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
    }
}
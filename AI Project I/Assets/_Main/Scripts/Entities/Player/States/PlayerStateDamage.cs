using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDamage<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDead;
        private float _timeElapsed;

        public PlayerStateDamage(in T inIdle, in T inMoving)// inDead
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }

        public override void Awake()
        {
            base.Awake();
            if (!Model.IsAlive())
            {
                Fsm.Transitions(_inDead);
            }
            View.PlayTargetAnimation(Model.GetData().HitAnimation.name);
            _timeElapsed = 0;
        }

        public override void Execute()
        {
            base.Execute();
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed >= Model.GetData().HitAnimation.Duration)
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
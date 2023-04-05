using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDamage<T> : EnemyStateBase<T>
    {
        private float _timeElapsed;
        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().HitAnimation.EventHash);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetCurrentTimer() > 0)
            {
                Model.RunTimer();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}
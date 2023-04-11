using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDamage<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            if (!Model.IsAlive())
            {
                Tree.Execute();
            }
            View.PlayTargetAnimation(Model.GetData().HitAnimation.EventHash);
            var timer = Model.GetData().HitAnimation.Duration;
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
                Tree.Execute();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}
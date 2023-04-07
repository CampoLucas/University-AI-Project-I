using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateIdle<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            var timer = Model.GetRandomTime();
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetCurrentTimer() > 0)
            {
                Model.RunTimer();
            }

            View.UpdateMovementValues(0);
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}
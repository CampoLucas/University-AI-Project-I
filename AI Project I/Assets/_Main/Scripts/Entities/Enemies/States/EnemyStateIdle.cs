using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateIdle<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            var timer = Model.GetRandomTime(3);
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.HasTakenDamage())
            {
                Tree.Execute();
            }
            
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
            }
            else
            {
                Tree.Execute();
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
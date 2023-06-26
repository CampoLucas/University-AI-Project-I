using UnityEngine;

namespace Game.Enemies.States
{
    public class WizardStateDamage<T> : WizardStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().HitAnimation.EventHash);
            var timer = Model.GetData().HitAnimation.Duration;
            Model.SetTimer(timer);
            Model.SetFollowing(true);
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
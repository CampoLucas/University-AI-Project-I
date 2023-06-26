using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStateIdle<T> : SlimeStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetTimer(1.5f);
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

using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStateSpin<T> : SlimeStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            var timer = Model.GetRandomTime(2f);
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            
            Model.RunJumpDelay();
            
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
                Model.Spin();
                Model.Move(Vector3.zero);
            }
            else
            {
                Tree.Execute();
            }
            
        }

        public override void Sleep()
        {
            base.Sleep();
        }
    }
}
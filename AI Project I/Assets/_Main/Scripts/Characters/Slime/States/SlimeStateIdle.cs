﻿using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStateIdle<T> : SlimeStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            var timer = Model.GetRandomTime(5f);
            if (timer < 2)
                timer = 1.5f;
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
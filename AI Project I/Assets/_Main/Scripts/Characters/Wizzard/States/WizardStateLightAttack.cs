﻿namespace Game.Enemies.States
{
    public class WizardStateLightAttack<T> : WizardStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            
            Attack();
        }

        public override void Execute()
        {
            base.Execute();
            var dir = Controller.Player.transform.position - Model.transform.position;
            Model.Rotate(dir.normalized);
            
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
            CancelAttack();
        }

        protected virtual void Attack()
        {
            Model.LightAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().LightAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().LightAttack01.Duration;
            Model.SetTimer(timer);
        }
        
        protected virtual void CancelAttack()
        {
            Model.CancelLightAttack();
        }
    }
}
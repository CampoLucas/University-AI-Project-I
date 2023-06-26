using UnityEngine;

namespace Game.Enemies.States
{
    public class WizardStateDeath<T> : WizardStateBase<T>
    {
        
        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().DeathAnimation.name);
            UnsubscribeAll();
        }
    }
}
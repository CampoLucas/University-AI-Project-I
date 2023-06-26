using _Main.Scripts.VisionCone;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDeath<T> : EnemyStateBase<T>
    {
        
        public override void Awake()
        {
            base.Awake();
            Model.SetVisionConeColor(VisionConeEnum.Nothing);
            View.PlayTargetAnimation(Model.GetData().DeathAnimation.name);
            UnsubscribeAll();
        }
    }
}
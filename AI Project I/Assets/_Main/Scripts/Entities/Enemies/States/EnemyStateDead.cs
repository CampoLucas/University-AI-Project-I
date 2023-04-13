using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDeath<T> : EnemyStateBase<T>
    {
        
        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().DeathAnimation.name);
        }
    }
}
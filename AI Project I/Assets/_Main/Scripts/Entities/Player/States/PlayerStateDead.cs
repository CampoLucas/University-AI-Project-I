using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDead<T> : PlayerStateBase<T>
    {
        
        public override void Awake()
        {
            base.Awake();
            View.PlayTargetAnimation(Model.GetData().DeathAnimation.name);
        }
    }
}
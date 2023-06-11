using UnityEngine;

namespace Game.Entities.Slime.States
{
    public class SlimeStateMove<T> : SlimeStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();

            var dir = Model.GetFlockingDir().normalized;
            
            Model.Move(dir);
        }
    }
}
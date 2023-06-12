using UnityEngine;

namespace Game.Entities.Slime.States
{
    public class SlimeStateMove<T> : SlimeStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();

            var dir = Model.GetFlockingDir();
            
            Model.LookDir(dir);
            Model.Move(Model.Front);
        }
    }
}
using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStateMove<T> : SlimeStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();
            
            
            Tree.Execute();
            if (Model.ReachedWaypoint())
            {
                Model.ChangeWaypoint();
            }
            
            Model.FollowTarget(Model.GetNextWaypoint(), Controller.GetAvoidance(), Controller.GetFlocking());
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero,0);
        }
    }
}
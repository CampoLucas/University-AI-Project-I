using Game.Scripts.VisionCone;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateFollowRoute<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetMovement(Model.GetWalkingMovement());
            Model.SetVisionConeColor(VisionConeEnum.Clear);
        }

        public override void Execute()
        {
            base.Execute();

            Tree.Execute();
            if (Model.ReachedWaypoint())
            {
                Model.ChangeWaypoint();
            }
            Model.FollowTarget(Model.GetNextWaypoint(), Controller.GetObsAvoid());
            View.UpdateMovementValues(0.5f);
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
            //View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
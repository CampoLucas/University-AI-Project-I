using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateFollowRoute<T> : EnemyStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();

            Tree.Execute();
            if (Model.ReachedWaypoint())
            {
                Model.ChangeWaypoint();
            }
            Model.FollowTarget(Model.GetNextWaypoint(), 0.5f);
            View.UpdateMovementValues(0.5f);
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero, Model.GetMoveAmount());
            //View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
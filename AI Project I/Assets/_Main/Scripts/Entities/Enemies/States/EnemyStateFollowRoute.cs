using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateFollowRoute<T> : EnemyStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();


            if (Model.IsAlive())
            {
                Tree.Execute();
                if (Model.ReachedWaypoint())
                {
                    Model.ChangeWaypoint();
                }
                Model.FollowTarget(Model.GetNextWaypoint(), 0.5f);
                View.UpdateMovementValues(0.5f);
            }
            else
            {
                Tree.Execute();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero, Model.GetMoveAmount());
            //View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
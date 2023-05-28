using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateSeek<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetFollowing(true);
            Model.SetMovement(Model.GetRunningMovement());
        }

        public override void Execute()
        {
            base.Execute();

            Tree.Execute();
            Model.FollowTarget(Controller.GetSeek(), Controller.GetObsAvoid());
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
            View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
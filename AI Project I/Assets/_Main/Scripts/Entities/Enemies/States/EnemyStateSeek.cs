using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateSeek<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetFollowing(true);
        }

        public override void Execute()
        {
            base.Execute();

            Tree.Execute();
            Model.FollowTarget(Model.GetSeek(), Model.GetMoveAmount());
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero, Model.GetMoveAmount());
            View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStatePursuit<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetTimer(Random.Range(2f, 6f));
            Model.SetMovement(Model.GetRunningMovement());
        }


        public override void Execute()
        {
            base.Execute();
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
            }
            else
            {
                Model.SetFollowing(false);
            }
            Tree.Execute();
            Model.FollowTarget(Controller.GetPursuit(), Controller.GetObsAvoid());
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
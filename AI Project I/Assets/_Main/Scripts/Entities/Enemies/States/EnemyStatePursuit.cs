using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStatePursuit<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.SetTimer(Random.Range(2f, 6f));
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
            Model.FollowTarget(Model.GetPursuit(), Model.GetMoveAmount());
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
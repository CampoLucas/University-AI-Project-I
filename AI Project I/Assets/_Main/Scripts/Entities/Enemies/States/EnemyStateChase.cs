using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateChase<T> : EnemyStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();

            if (Model.IsAlive())
            {
                Model.FollowTarget(Controller.Target, Model.GetMoveAmount());
                View.UpdateMovementValues(Model.GetMoveAmount());
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
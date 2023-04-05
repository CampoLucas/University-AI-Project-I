using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateChase<T> : EnemyStateBase<T>
    {
        public override void Execute()
        {
            base.Execute();
            
            Model.FollowTarget(Controller.Target);
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
            //View.UpdateMovementValues(Model.GetMoveAmount());
        }
    }
}
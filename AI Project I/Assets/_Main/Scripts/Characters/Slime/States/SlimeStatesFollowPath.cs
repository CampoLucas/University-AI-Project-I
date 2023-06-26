using UnityEngine;

namespace Game.Entities.Slime.States
{
    public sealed class SlimeStatesFollowRoute<T> : SlimeStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            
            Model.GetRandomNode();
            CalculatePath();
        }

        public override void Execute()
        {
            base.Execute();
            
            Model.RunJumpDelay();
            
            if (Model.HasReachedTarget())
            {
                Tree.Execute();
            }
            else
            {
                Follow();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
            Model.ClearTarget();
        }

        private void CalculatePath()
        {
            var pos = Model.transform.position;
            var targetPos = Model.GetPathfinder().Target.position;
            Model.SetNodes(pos, targetPos);
            Model.CalculatePath();
        }
        
        private void Follow()
        {
            Vector3 flockingDir = Controller.GetFlocking().GetDir();
            Model.FollowTarget(Model.GetPathfinder(), flockingDir, Controller.GetAvoidance());
        }
    }
}
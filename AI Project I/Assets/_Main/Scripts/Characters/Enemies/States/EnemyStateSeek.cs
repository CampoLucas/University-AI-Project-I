using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateSeek<T> : EnemyStatePursuit<T>
    {
        public EnemyStateSeek(ISteering steering, ISteering obsAvoidance) : base(steering, obsAvoidance) {}

        public override void Awake()
        {
            base.Awake();
            Model.SetTimer(Random.Range(8f, 16f));
            CalculatePath();
            Model.SetTarget(Controller.Player.transform);
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
        }

        private void CalculatePath()
        {
            var pos = Model.transform.position;
            var targetPos = Controller.Player.transform.position;

            Model.SetNodes(pos, targetPos);
            Model.CalculatePath();
        }

        protected override void Follow()
        {
            if (!Model.IsTargetInRange())
            {
                Logging.LogPathfinder($"Is target in range: {Model.IsTargetInRange()}");
                CalculatePath();
            }
            Model.FollowTarget(Model.GetPathfinder(), Steering, ObsAvoidance);
        }
    }
}
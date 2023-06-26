﻿using _Main.Scripts.VisionCone;
using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStatePursuit<T> : EnemyStateBase<T>
    {
        protected ISteering Steering;
        protected ISteering ObsAvoidance;

        public EnemyStatePursuit(ISteering steering, ISteering obsAvoidance)
        {
            Steering = steering;
            ObsAvoidance = obsAvoidance;
        }

        public override void Awake()
        {
            base.Awake();
            Model.SetFollowing(true);
            Model.SetMovement(Model.GetRunningMovement());
            Model.SetVisionConeColor(VisionConeEnum.InSight);
        }


        public override void Execute()
        {
            base.Execute();
            
            Tree.Execute();
            Follow();
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
            View.UpdateMovementValues(Model.GetMoveAmount());
        }

        protected virtual void Follow()
        {
            Model.FollowTarget(Steering, ObsAvoidance);
        }

        public override void Dispose()
        {
            base.Dispose();
            Steering = null;
            ObsAvoidance = null;
        }
    }
}
using System;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.SO;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeModel : EntityModel, IBoid
    {
        [Range(1, 25)] 
        [SerializeField] private float boidRadius = 5;

        private EnemySO _data;
        private SlimeFlockingManager _slimeFlocking;
        private PathToFollow _path;
        
        private bool _isPathNull;

        public Vector3 Position => transform.position;
        public Vector3 Front => transform.forward;
        public float Radius => boidRadius;

        protected override void Awake()
        {
            base.Awake();
            _data = GetData<EnemySO>();
            _slimeFlocking = GetComponent<SlimeFlockingManager>();
            _path = GetComponent<PathToFollow>();
        }

        private void Start()
        {
            _isPathNull = _path == null;
        }

        public void FollowTarget(Vector3 target, ISteering avoidance)
        {
            var dir = (target - transform.position).normalized + avoidance.GetDir() * _data.ObsMultiplier;
            dir.y = 0;
            Move(transform.forward);
            Rotate(dir);
        }
        
        public void FollowTarget(Vector3 target, ISteering avoidance, FlockingManager flocking)
        {
            var steering = ((avoidance.GetDir() * _data.ObsMultiplier) + flocking.GetDir()) / 2;
            var dir = (target - transform.position).normalized + steering.normalized;
            dir.y = 0;
            Move(transform.forward);
            Rotate(dir);
        }


        #region Path

        public Vector3 GetNextWaypoint()
        {
            return _isPathNull ? Vector3.zero : _path.GetNextWaypoint();
        }

        public bool HasARoute()
        {
            return _isPathNull ? false : _path.Path;
        }

        public bool ReachedWaypoint()
        {
            return !_isPathNull && _path.ReachedWaypoint();
        }

        public void ChangeWaypoint()
        {
            if (_isPathNull) return;
                
            _path.ChangeWaypoint();
        }

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Position, Radius);
        }
    }
}
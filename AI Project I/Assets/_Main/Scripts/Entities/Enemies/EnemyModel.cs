﻿using System;
using UnityEngine;
using Game.Entities;
using Game.Entities.Steering;
using Game.Interfaces;
using Game.Player;
using Game.SO;
using Unity.VisualScripting;
using UnityEditor;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyModel : EntityModel
    {
        [SerializeField] private PlayerModel player;
        private EnemySO _data;
        private FieldOfView _fieldOfView;
        private PathToFollow _path;
        private InRange _range;
        private Vector3 _direction = Vector3.zero;
        private bool _wasFollowing; //ToDo: Si resive daño o ve al player que se haga verdadero, si es verdadero y el player no esta en el lineofsight por x cantidad de tiempo que se haga falso;
        private ISteering _seek;
        private ISteering _pursuit;
        private ISteering _evade;
        private ISteering _flee;
        private ISteering _obsAvoidance;

        private void InitSteering()
        {
            var transform1 = transform;
            var transform2 = player.transform;
            _seek = new Seek(transform1, transform2);
            _flee = new Flee(transform1, transform2);
            _pursuit = new Pursuit(transform1, player, _data.PursuitTime);
            _evade = new Evade(transform1, player, _data.PursuitTime);
            _obsAvoidance = new ObstacleAvoidance(transform1, _data.ObsAngle, _data.ObsRange, _data.MaxObs, _data.ObsMask);
        }
        protected override void Awake()
        {
            base.Awake();
            _data = GetData<EnemySO>();
            _fieldOfView = new FieldOfView(_data, transform);
            _path = GetComponent<PathToFollow>();
            _range = new InRange(transform);
            InitSteering();
        }

        private void Start()
        {
            Spawn();
        }

        public float GetMoveAmount() => Mathf.Clamp01(Mathf.Abs(_direction.x) + Mathf.Abs(_direction.z));

        public override void Move(Vector3 dir, float moveAmount)
        {
            _direction = dir;
            base.Move(_direction, moveAmount);
        }

        public bool CheckRange(Transform target) => _fieldOfView.CheckRange(target);
        public bool CheckAngle(Transform target) => _fieldOfView.CheckAngle(target);
        public bool CheckView(Transform target) => _fieldOfView.CheckView(target);

        public void Spawn()
        {
            if (_path)
            {
                transform.position = _path.GetCurrentPoint();
                transform.rotation = Quaternion.LookRotation(_path.GetWaypointDirection());
            }
            
        }

        #region Waypoint

        public Vector3 GetWaypointDirection() => _path.GetWaypointDirection();
        public Vector3 GetNextWaypoint() => _path.GetNextWaypoint();
        public bool HasARoute() => _path;
        public bool ReachedWaypoint() => _path.ReachedWaypoint();
        public void ChangeWaypoint() => _path.ChangeWaypoint();

        #endregion
        
        public void FollowTarget(Transform target, float moveAmount)
        {
            var transform1 = transform;
            var dir = (target.position - transform1.position).normalized;
            Move(transform1.forward, moveAmount);
            Rotate(dir);
        }

        public void FollowTarget(Vector3 target, float moveAmount)
        {
            var transform1 = transform;
            var dir = (target - transform1.position).normalized;
            Move(transform1.forward, moveAmount);
            Rotate(dir);
        }
        
        public void FollowTarget(ISteering steering, float moveAmount)
        {
            var dir = (steering.GetDir() + _obsAvoidance.GetDir() * _data.ObsMultiplier).normalized;
            dir.y = 0;
            Move(transform.forward, moveAmount);
            Rotate(dir);
        }

        public ISteering GetSeek() => _seek;
        public ISteering GetPursuit() => _pursuit;
        public ISteering GetFlee() => _flee;
        public ISteering GetEvade() => _evade;

        

        #region Desition Tree Questions

        public bool TargetInRange(Transform target) => _range.GetBool(target, _data.AttackRange);
        public bool IsPlayerAlive() => player && player.IsAlive();
        #endregion


        protected override void OnDestroy()
        {
            base.OnDestroy();
            player = null;
            _fieldOfView.Dispose();
            _fieldOfView = null;
            _data = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetData<EnemySO>().AttackRange);
        }

        private void OnDrawGizmos()
        {
            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            #region FOV

            Gizmos.color = Color.red;
            var halfFOV = GetData<EnemySO>().FovAngle / 2f;
            var leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

            
            var leftRayDirection = leftRayRotation * forward;
            var rightRayDirection = rightRayRotation * forward;

            
            Gizmos.DrawRay(position, leftRayDirection * GetData<EnemySO>().FovRange);
            Gizmos.DrawRay(position, rightRayDirection * GetData<EnemySO>().FovRange);

            Handles.color = new Color(1f, 0f, 0f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.up, leftRayDirection, GetData<EnemySO>().FovAngle, GetData<EnemySO>().FovRange);
            Handles.color = Color.red;
            Handles.DrawWireArc(position, Vector3.up, leftRayDirection, GetData<EnemySO>().FovAngle, GetData<EnemySO>().FovRange);

            #endregion

            #region ObsAvoidance

            Gizmos.color = Color.blue;
            var halfObs = GetData<EnemySO>().ObsAngle / 2f;
            var obsLeftRayRotation = Quaternion.AngleAxis(-halfObs, Vector3.up);
            var obsRightRayRotation = Quaternion.AngleAxis(halfObs, Vector3.up);

            var obsLeftRayDirection = obsLeftRayRotation * forward;
            var obsRightRayDirection = obsRightRayRotation * forward;

            
            Gizmos.DrawRay(position, obsLeftRayDirection * GetData<EnemySO>().ObsRange);
            Gizmos.DrawRay(position, obsRightRayDirection * GetData<EnemySO>().ObsRange);

            Handles.color = new Color(0f, 0f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObsAngle, GetData<EnemySO>().ObsRange);
            Handles.color = Color.blue;
            Handles.DrawWireArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObsAngle, GetData<EnemySO>().ObsRange);

            #endregion
        }
    }
}
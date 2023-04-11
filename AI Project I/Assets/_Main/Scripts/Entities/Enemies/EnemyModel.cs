using System;
using UnityEngine;
using Game.Entities;
using Game.Player;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyModel : EntityModel
    {
        [SerializeField] private float attackRange;
        [SerializeField] private PlayerModel player;
        private FieldOfView _fieldOfView;
        private PathToFollow _path;
        private InRange _range;
        private Vector3 _direction = Vector3.zero;
        private bool _wasFollowing; //ToDo: Si resive daño o ve al player que se haga verdadero, si es verdadero y el player no esta en el lineofsight por x cantidad de tiempo que se haga falso;

        protected override void Awake()
        {
            base.Awake();
            _fieldOfView = GetComponent<FieldOfView>();
            _path = GetComponent<PathToFollow>();
            _range = new InRange(transform);
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

        #region Desition Tree Questions

        public bool TargetInRange(Transform target) => _range.GetBool(target, attackRange);
        public bool IsPlayerAlive() => player && player.IsAlive();
        #endregion
        

        //public bool 

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
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
        private Vector3 _direction = Vector3.zero;
        private bool _isPlayerInSight;

        protected override void Awake()
        {
            base.Awake();
            _fieldOfView = GetComponent<FieldOfView>();
            _path = GetComponent<PathToFollow>();
        }

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            if (_path.ReachedWaypoint())
            {
                _path.ChangeWaypoint();
            }
            FollowTarget(_path.GetNextWaypoint());
            
        }

        public float GetMoveAmount() => Mathf.Clamp01(Mathf.Abs(_direction.x) + Mathf.Abs(_direction.z));

        public override void Move(Vector3 dir)
        {
            _direction = dir;
            base.Move(_direction);
        }

        public bool CheckRange(Transform target) => _fieldOfView.CheckRange(target);
        public bool CheckAngle(Transform target) => _fieldOfView.CheckAngle(target);
        public bool CheckView(Transform target) => _fieldOfView.CheckView(target);

        public void Spawn()
        {
            transform.position = _path.GetCurrentPoint();
            transform.rotation = Quaternion.LookRotation(_path.GetWaypointDirection());
        }
        public void FollowTarget(Transform target)
        {
            var transform1 = transform;
            var dir = (target.position - transform1.position).normalized;
            Move(transform1.forward);
            Rotate(dir);
        }

        public void FollowTarget(Vector3 target)
        {
            var transform1 = transform;
            var dir = (target - transform1.position).normalized;
            Move(transform1.forward);
            Rotate(dir);
        }

        public bool IsInAttackingRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < attackRange;
        }
        public bool IsPlayerInSight() => _isPlayerInSight;
        public void SetPlayerInSight(bool isInSight) => _isPlayerInSight = isInSight;

        public bool IsPlayerAlive() => player && player.IsAlive();
        
        //public bool 

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
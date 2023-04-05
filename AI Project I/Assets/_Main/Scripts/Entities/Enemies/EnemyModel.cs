using UnityEngine;
using Game.Entities;

namespace Game.Enemies
{
    public class EnemyModel : EntityModel
    {
        [SerializeField] private float attackRange;
        private FieldOfView _fieldOfView;
        private WaitTimer _waitTimer;
        private Vector3 _direction = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            _fieldOfView = GetComponent<FieldOfView>();
            _waitTimer = GetComponent<WaitTimer>();
        }

        public float GetMoveAmount() => Mathf.Clamp01(Mathf.Abs(_direction.x) + Mathf.Abs(_direction.z));

        public override void Move(Vector3 dir)
        {
            _direction = dir;
            base.Move(_direction);
        }

        #region Field of view

        public bool CheckRange(Transform target) => _fieldOfView && _fieldOfView.CheckRange(target);
        public bool CheckAngle(Transform target) => _fieldOfView && _fieldOfView.CheckAngle(target);
        public bool CheckView(Transform target) => _fieldOfView && _fieldOfView.CheckView(target);

        #endregion

        
        
        #region Actions

        public void FollowTarget(Transform target)
        {
            var transform1 = transform;
            var dir = (target.position - transform1.position).normalized;
            Move(transform1.forward);
            Rotate(dir);
        }

        public void Scape()
        {
            
        }

        public void Patrol()
        {
            
        }

        public void Idle()
        {
            
        }

        #endregion

        #region Questions

        public bool TriesToScape() => Random.Range(0, 11) == 10;
        public bool CloseEnoughToAttack(Transform target) => Vector3.Distance(transform.position, target.position) < attackRange;
        public bool SeePlayer(Transform target) => CheckRange(target) && CheckAngle(target) && CheckView(target);

        #endregion
        
    }
}
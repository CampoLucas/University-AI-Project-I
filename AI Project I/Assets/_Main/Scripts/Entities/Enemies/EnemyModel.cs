using UnityEngine;
using Game.Entities;

namespace Game.Enemies
{
    public class EnemyModel : EntityModel
    {
        [SerializeField] private float attackRange;
        private FieldOfView _fieldOfView;

        protected override void Awake()
        {
            base.Awake();
            _fieldOfView = GetComponent<FieldOfView>();
        }

        public bool CheckRange(Transform target) => _fieldOfView && _fieldOfView.CheckRange(target);
        public bool CheckAngle(Transform target) => _fieldOfView && _fieldOfView.CheckAngle(target);
        public bool CheckView(Transform target) => _fieldOfView && _fieldOfView.CheckView(target);

        public void FollowTarget(Transform target)
        {
            var dir = (target.position - transform.position).normalized;
            Move(transform.forward);
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
        
        public bool TriesToScape() => Random.Range(0, 11) == 10;
        public bool CloseEnoughToAttack(Transform target) => Vector3.Distance(transform.position, target.position) < attackRange;
        public bool SeePlayer(Transform target) => CheckRange(target) && CheckAngle(target) && CheckView(target);
    }
}
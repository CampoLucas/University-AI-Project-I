using System;
using Game.Enemies;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.Pathfinding;
using Game.SO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities.Slime
{
    public class SlimeModel : EnemyModel, IBoid
    {
        [SerializeField] private Transform leader;
        
        private Transform _cube;
        private BoxCollider _boxCollider;
        private CapsuleCollider _capsuleCollider;
        private float _currJumpDelay;
        private SlimeFlockingManager _slimeFlocking;
        private SlimeSO _slimeData;
        private bool _isDataNull;
        public Vector3 Position => transform.position;
        public Vector3 Front => transform.forward;
        public float Radius => GetBoidRadius();
        public int BLevel => GetCurrentLevel();
        public bool CanJump { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            _slimeData = GetData<SlimeSO>();
            _isDataNull = _slimeData == null;

            _cube = transform.GetChild(0);
            _boxCollider = GetComponent<BoxCollider>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            
            CanJump = true;
        }

        private void Start()
        {
            SetSize();
        }

        #region Movement

        /*public override void Move(Vector3 dir)
        {
            Jump(dir);
        }*/

        public void Jump(Vector3 dir, float multiplier = 1)
        {
            if (!CanJump || _currJumpDelay > 0) return;
            
            var jumpForce = _slimeData.JumpForce;
            var finalDir = (dir + transform.up).normalized;
            Rigidbody.AddForce(finalDir * (jumpForce * multiplier), ForceMode.Impulse);
            
            var targetVelocity = dir * _slimeData.MoveSpeed;
            var cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, Vector3.zero);
            Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, cachedProjectedVelocity, _slimeData.MoveLerpSpeed);
        }

        public void Spin()
        {
            var rotSpeed = _slimeData.RotSpeed * _slimeData.RotationMultiplier;
            transform.Rotate( transform.up * (rotSpeed * Time.deltaTime));
        }

        public void RunJumpDelay()
        {
            if(CanJump)
                _currJumpDelay -= Time.deltaTime;
        }

        public void ClearJumpDelay()
        {
            _currJumpDelay = 0;
        }

        private void ResetJumpDelay()
        {
            _currJumpDelay = _slimeData.JumpDelay;
        }

        #endregion

        #region Size

        public void IncreaseSize()
        {
            var currScale = transform.localScale.x;
            var speed = _slimeData.GrowSpeed * Time.deltaTime;

            currScale += speed;

            if (currScale > GetTargetSize())
                currScale = GetTargetSize();
            
            var newSize = Vector3.one * currScale;
            _cube.localScale = newSize;
            _boxCollider.size = newSize;
            _capsuleCollider.radius = currScale;
        }

        private void SetSize()
        {
            if (_isDataNull) return;

            var size =GetTargetSize() * Vector3.one;
            _cube.localScale = size;
            _boxCollider.size = size;
            _capsuleCollider.radius = GetTargetSize()/2;
        }

        private float GetTargetSize()
        {
            return (float)GetCurrentLevel() / 10 * 2.5f;
        }

        #endregion

        #region Misc

        private float GetBoidRadius()
        {
            return _isDataNull ? 0 : _slimeData.BoidRadius;
        }

        public Transform GetLeader()
        {
            return leader;
        }

        #endregion
        
        #region Nodes

        public bool HasTargetNode()
        {
            return GetPathfinder().Target;
        }

        public void GetRandomNode()
        {
            var randPoint = Random.insideUnitSphere;
            var finalValue = randPoint * _slimeData.RandomPointRadius;
            finalValue.y = 0;
            var targetNode = NodeGrid.GetInstance().GetClosestNode(finalValue);
            
            SetTarget(targetNode.transform);
        }

        public void ClearTarget()
        {
            SetTarget(null);
        }

        public bool HasReachedTarget()
        {
            var targetPos = GetPathfinder().Target.position;
            targetPos.y = 0;
            var distance = Vector3.Distance(transform.position, targetPos);

            return distance <= _slimeData.AttackRange;
        }

        #endregion

        #region Collision

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                CanJump = true;
                ResetJumpDelay();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                CanJump = false;
            }
        }

        #endregion
        
        #region Gizmos

        protected override void OnDrawGizmosSelected()
        {
            //Boids
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().BoidRadius);
            
            //Predator
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().PredatorRange);
            
            //RandomPoint
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().RandomPointRadius);
            
            //Separate
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().PersonalRange);
        }

        protected override void OnDrawGizmos()
        {
            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            
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

        #endregion
    
    }
}
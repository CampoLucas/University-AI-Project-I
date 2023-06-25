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
        private float _currJumpDelay;
        private SlimeFlockingManager _slimeFlocking;
        private SlimeSO _slimeData;
        private bool _isDataNull;
        public Vector3 Position => transform.position;
        public Vector3 Front => transform.forward;
        public float Radius => GetBoidRadius();
        public int BLevel => Level;
        public bool CanJump { get; private set; }
        private float TargetSize =>(float)Level/10 * 2.5f;
        

        protected override void Awake()
        {
            base.Awake();
            _slimeData = GetData<SlimeSO>();
            _isDataNull = _slimeData == null;
            CanJump = true;
        }

        private void Start()
        {
            SetSize();
        }

        #region Movement

        public override void Move(Vector3 dir)
        {
            if (!CanJump || _currJumpDelay > 0) return;
            
            var jumpForce = GetData<SlimeSO>().JumpForce;
            var finalDir = (dir + transform.up).normalized;
            Rigidbody.AddForce(finalDir * jumpForce, ForceMode.Impulse);
            
            var targetVelocity = dir * GetData<SlimeSO>().MoveSpeed;
            var cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, Vector3.zero);
            Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, cachedProjectedVelocity, GetData<SlimeSO>().MoveLerpSpeed);
            //CanJump = false;
        }

        public void Spin()
        {
            var rotSpeed = GetData<SlimeSO>().RotSpeed * GetData<SlimeSO>().RotationMultiplier;
            transform.Rotate( transform.up * (rotSpeed * Time.deltaTime));
        }

        public void RunJumpDelay()
        {
            if(CanJump)
                _currJumpDelay -= Time.deltaTime;
        }

        private void ResetJumpDelay()
        {
            _currJumpDelay = GetData<SlimeSO>().JumpDelay;
        }

        #endregion

        #region Size

        public void IncreaseSize()
        {
            var currScale = transform.localScale.x;
            var speed = GetData<SlimeSO>().GrowSpeed * Time.deltaTime;

            currScale = Mathf.Clamp(currScale + speed, currScale, TargetSize);

            transform.localScale = Vector3.one * currScale;
        }

        public bool HasTargetSize()
        {
            return Math.Abs(transform.localScale.x - TargetSize) < 0.1f;
        }

        private void SetSize()
        {
            if (_isDataNull) return;

            transform.localScale = TargetSize * Vector3.one;
        }
        

        #endregion

        #region Misc

        private float GetBoidRadius()
        {
            return _isDataNull ? 0 : _slimeData.BoidRadius;
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
            var finalValue = randPoint * GetData<SlimeSO>().RandomPointRadius;
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

            return distance <= GetData<SlimeSO>().AttackRange;
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().BoidRadius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().PredatorRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().RandomPointRadius);
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
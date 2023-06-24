using System;
using Game.Enemies;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.SO;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities.Slime
{
    public class SlimeModel : EnemyModel, IBoid
    {
        private float _jumpDelay;
        private SlimeSO _slimeData;
        private bool _isDataNull;

        private SlimeFlockingManager _slimeFlocking;
        public Vector3 Position => transform.position;
        public Vector3 Front => transform.forward;
        public float Radius => GetBoidRadius();

        protected override void Awake()
        {
            base.Awake();
            _slimeData = GetData<SlimeSO>();
            _isDataNull = _slimeData == null;
        }

        private void Start()
        {
            SetSize();
        }

        private void Update()
        {
            if (_jumpDelay > 0)
            {
                _jumpDelay -= Time.deltaTime;
            }
        }

        public override void Move(Vector3 dir)
        {
            if (_jumpDelay > 0) return;

            var jumpForce = GetData<SlimeSO>().JumpForce;
            var finalDir = (dir + transform.up).normalized;
            Rigidbody.AddForce(finalDir * jumpForce, ForceMode.Impulse);
            ResetJump();
        }

        private void ResetJump()
        {
            _jumpDelay = GetData<SlimeSO>().JumpDelay;
        }

        private float GetBoidRadius()
        {
            return _isDataNull ? 0 : _slimeData.BoidRadius;
        }

        private void SetSize()
        {
            if (_isDataNull) return;

            var size = Random.Range(_slimeData.MinSize, _slimeData.MaxSize);
            
            transform.localScale = new Vector3(size,size,size);
        }

        public bool HasTargetNode()
        {
            return GetPathfinder().Target;
        }


        #region Gizmos

        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().BoidRadius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, GetData<SlimeSO>().PredatorRange);
        }

        protected override void OnDrawGizmos()
        {
            
        }

        #endregion

    }
}
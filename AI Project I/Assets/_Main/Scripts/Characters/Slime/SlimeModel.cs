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
    }
}
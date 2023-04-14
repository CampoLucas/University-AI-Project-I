using System;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Enemies
{
    public class FieldOfView : IFieldOfView
    {
        private EnemySO _data;
        private Transform _origin;

        public FieldOfView(EnemySO data, Transform origin)
        {
            _data = data;
            _origin = origin;
        }
        
        public bool CheckRange(Transform target)
        {
            var distance = Vector3.Distance(_origin.position, target.position);
            return distance < _data.FovRange;
        }

        public bool CheckAngle(Transform target)
        {
            var forward = _origin.forward;
            var dirToTarget = target.position - _origin.position;
            var angleToTarget = Vector3.Angle(forward, dirToTarget);
            return _data.FovAngle / 2 > angleToTarget;
        }

        public bool CheckView(Transform target)
        {
            var position = _origin.position;
            var diff = target.position - position;
            var dirToTarget = diff.normalized;
            var distanceToTarget = diff.magnitude;

            return !Physics.Raycast(position, dirToTarget, out var hit, distanceToTarget, _data.FovMask);
        }

        public void Dispose()
        {
            _data = null;
            _origin = null;
        }
    }
}
using Game.Data;
using Game.Interfaces;
using Game.SO;
using UnityEngine;


namespace Game.Enemies
{
    /// <summary>
    /// This class is used to detect if a target is within the enemy's field of view.
    /// </summary>
    public class FieldOfView : IFieldOfView
    {
        private FieldOfViewData _data;
        private Transform _origin;

        public FieldOfView(FieldOfViewData data, Transform origin)
        {
            _data = data;
            _origin = origin;
        }
        
        public bool CheckRange(Transform target)
        {
            var distance = Vector3.Distance(_origin.position, target.position);
            return distance < _data.Range;
        }

        public bool CheckAngle(Transform target)
        {
            var forward = _origin.forward;
            var dirToTarget = target.position - _origin.position;
            var angleToTarget = Vector3.Angle(forward, dirToTarget);
            return _data.Angle / 2 > angleToTarget;
        }

        public bool CheckView(Transform target)
        {
            var position = _origin.position;
            var diff = target.position - position;
            var dirToTarget = diff.normalized;
            var distanceToTarget = diff.magnitude;

            return !Physics.Raycast(position, dirToTarget, out var hit, distanceToTarget, _data.Mask);
        }

        /// <summary>
        /// Method used to nullify the references.
        /// </summary>
        public void Dispose()
        {
            _data = null;
            _origin = null;
        }
    }
}
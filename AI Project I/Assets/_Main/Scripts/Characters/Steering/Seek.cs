using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Seek : ISteering
    {
        private Transform _target;
        private Transform _origin;

        public Seek(Transform origin, Transform target)
        {
            _target = target;
            _origin = origin;
        }

        /// <summary>
        /// A method that returns the direction to the target.
        /// </summary>
        public virtual Vector3 GetDir()
        {
            return (_target.position - _origin.position).normalized;
        }

        public void Dispose()
        {
            _target = null;
            _origin = null;
        }
    }
}

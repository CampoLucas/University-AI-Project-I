using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Pursuit : ISteering
    {
        private Transform _origin;
        private EntityModel _target;
        private readonly float _time;

        public Pursuit(Transform origin, EntityModel target, float time)
        {
            _origin = origin;
            _target = target;
            _time = time;
        }

        /// <summary>
        /// A method that returns the predicted direction of the target.
        /// It calculates de direction the target will reach depending on it's speed
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetDir()
        {
            var position = _target.transform.position;
            var position1 = _origin.position;
            var distance = Vector3.Distance(position1, position);
            // would it be good if the time is multiplied by the distance?
            var point = position +
                        _target.GetForward() * Mathf.Clamp(_target.GetSpeed() * _time, -distance, distance);
            var dir = (point - position1).normalized;
            return dir;
        }

        public void Dispose()
        {
            _origin = null;
            _target = null;
        }
    }
}

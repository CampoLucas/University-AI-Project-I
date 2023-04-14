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

        public virtual Vector3 GetDir()
        {
            var distance = Vector3.Distance(_origin.position, _target.transform.position);
            var point = _target.transform.position +
                        _target.GetForward() * Mathf.Clamp(_target.GetSpeed() * _time, -distance, distance);
            var dir = (point - _origin.position).normalized;
            return dir;
        }

        public void Dispose()
        {
            _origin = null;
            _target = null;
        }
    }
}

using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class ObstacleAvoidance : ISteering
    {
        private Transform _origin;
        private readonly LayerMask _mask;
        private readonly float _radius;
        private readonly float _angle;
        private Collider[] _obs;

        public ObstacleAvoidance(Transform origin, float angle, float radius, int maxObs, LayerMask mask)
        {
            _origin = origin;
            _angle = angle;
            _radius = radius;
            _mask = mask;
            _obs = new Collider[maxObs];
        }

        public Vector3 GetDir()
        {
            var obsCount = Physics.OverlapSphereNonAlloc(_origin.position, _radius, _obs, _mask);
            var dirToAvoid = Vector3.zero;
            var detectedObs = 0;
            for (var i = 0; i < obsCount; i++)
            {
                var curr = _obs[i];
                var position = _origin.position;
                var closestPoint = curr.ClosestPointOnBounds(position);
                var diffToPoint = closestPoint - position;
                var angleToObs = Vector3.Angle(_origin.forward, diffToPoint);
                if (angleToObs > _angle / 2) continue;
                var distance = diffToPoint.magnitude;
                detectedObs++;
                dirToAvoid += -(diffToPoint).normalized * (_radius - distance);
            }

            if (detectedObs != 0)
                dirToAvoid /= detectedObs;

            return dirToAvoid.normalized;
        }

        public void Dispose()
        {
            _origin = null;
            _obs = null;
        }
    }
}

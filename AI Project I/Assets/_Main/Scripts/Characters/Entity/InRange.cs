using UnityEngine;

namespace Game.Entities
{
    public class InRange
    {
        private readonly Transform _origin;
        
        public InRange(Transform origin)
        {
            _origin = origin;
        }

        public bool GetBool(Transform target, float radius)
        {
            if (!target)
            {
                return false;
            }
    
            var distanceToTarget = Vector3.Distance(_origin.position, target.position);
            return distanceToTarget <= radius;
        }
    }
}
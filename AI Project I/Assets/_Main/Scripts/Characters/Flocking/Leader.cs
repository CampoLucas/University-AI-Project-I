using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;

namespace Game.Entities.Flocking
{
    public class Leader : IFlocking
    {
        private float _multiplier;
        public Transform _target;

        public Leader(Transform target, float multiplier)
        {
            _target = target;
            _multiplier = multiplier;
        }

        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            return _target == null ? Vector3.zero : (_target.position - self.Position).normalized * _multiplier;
        }
    }

}
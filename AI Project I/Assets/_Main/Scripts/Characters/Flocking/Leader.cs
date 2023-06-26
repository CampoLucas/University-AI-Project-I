using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.SO;

namespace Game.Entities.Flocking
{
    public class Leader : IFlocking
    {
        private float _multiplier;
        public Transform _target;
        private SlimeSO _data;

        public Leader(Transform target, float multiplier)
        {
            _target = target;
            _multiplier = multiplier;
        }

        public Leader(Transform target, SlimeSO data)
        {
            _target = target;
            _data = data;
        }

        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            return _target == null ? Vector3.zero : (_target.position - self.Position).normalized * _data.LeaderMultiplier;
        }
    }

}
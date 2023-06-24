using System;
using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Predator : IFlocking
    {
        private readonly float _multiplier;
        private readonly float _predatorRange;
        private readonly LayerMask _whatIsPredator;
        readonly Collider[] _colliders;
        private int _selfLevel;
        private int _predLevel;

        public Predator(float multiplier, float predatorRange, int predatorMax, LayerMask whatIsPredator, float selfLevel)
        {
            _multiplier = multiplier;
            _predatorRange = predatorRange;
            _whatIsPredator = whatIsPredator;
            _colliders = new Collider[predatorMax];
        }
        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            int count = Physics.OverlapSphereNonAlloc(self.Position, _predatorRange, _colliders, _whatIsPredator);

            if (count < 1) return Vector3.zero;
            
            Vector3 dir = Vector3.zero;
            
            for (int i = 0; i < count; i++)
            {
                var diff = self.Position - _colliders[i].transform.position;
                dir += diff.normalized * (_predatorRange - diff.magnitude);
                
                if(!_colliders[i].TryGetComponent(out EntityModel predator)) continue;

                _predLevel = predator.Level;
            }

            var lvlDiff = _selfLevel - _predLevel;
            lvlDiff = lvlDiff switch
            {
                < 0 => 1,
                > 0 => -1,
                _ => 0
            };

            return dir.normalized * (_multiplier * lvlDiff);
        }
    }
}

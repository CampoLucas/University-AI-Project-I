using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public abstract class FlockingManager
    {
        protected IFlocking[] Flocking { private get; set; }
        private readonly List<IBoid> _boids;
        private readonly IBoid _self;
        private readonly Collider[] _colliders;
        private readonly LayerMask _whatIsBoid;
        private readonly float _multiplier;


        protected FlockingManager(IBoid self, int maxBoids, LayerMask whatIsBoid, float multiplier)
        {
            _self = self;
            _colliders = new Collider[maxBoids];
            _boids = new List<IBoid>();
            _whatIsBoid = whatIsBoid;
            _multiplier = multiplier;
        }
        

        public Vector3 GetDir()
        {
            _boids.Clear();
            
            int count = Physics.OverlapSphereNonAlloc(_self.Position, _self.Radius, _colliders, _whatIsBoid);

            for (int i = 0; i < count; i++)
            {
                if(!_colliders[i].TryGetComponent(out IBoid boid)) continue;
                
                _boids.Add(boid);
            }

            Vector3 dir = Vector3.zero;

            for (int i = 0; i < Flocking.Length; i++)
            {
                var currFlock = Flocking[i];
                dir += currFlock.GetDir(_boids, _self);
            }
            
            
            return dir.normalized * _multiplier;
        }
    }
}

using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public abstract class FlockingManager : MonoBehaviour
    {
        [Header("General")]
        [Range(1, 15)] [SerializeField] private int maxBoids;
        [SerializeField] private LayerMask whatIsBoid;

        protected IFlocking[] Flocking { get; set; }
        private List<IBoid> _boids;
        private IBoid _self;
        private Collider[] _colliders;

        private void Start()
        {
            _self = GetComponent<IBoid>();
            _boids = new List<IBoid>();
            _colliders = new Collider[maxBoids];
            SetFlocking();
        }

        protected abstract void SetFlocking();

        public Vector3 GetDir()
        {
            _boids.Clear();
            
            int count = Physics.OverlapSphereNonAlloc(_self.Position, _self.Radius, _colliders, whatIsBoid);

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
            
            return dir.normalized;
        }
    }
}

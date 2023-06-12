using System;
using System.Collections.Generic;
using Game.Entities.Flocking;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeFlockingManager : MonoBehaviour
    {
        [Header("General")]
        [Range(1, 15)] [SerializeField] private int maxBoids;
        [SerializeField] private LayerMask whatIsBoid;
        [Space]
        [Header("Predator")]
        [Range(1,10)][SerializeField] private float pRange;
        [Range(1,10)][SerializeField] private int pMax;
        [SerializeField] private LayerMask whatIsPredator;
        [Space]
        [Header("Multipliers")]
        [Range(0f,10f)][SerializeField] private float predatorMultiplier;
        [Range(0f,10f)][SerializeField] private float alignmentMultiplier;
        [Range(0f,10f)][SerializeField] private float cohesionMultiplier;
        
        private IFlocking[] _flocking;
        private List<IBoid> _boids;
        private IBoid _self;
        private Collider[] _colliders;


        private void Awake()
        {
            _self = GetComponent<IBoid>();
        }

        private void Start()
        {
            var predator = new Predator(predatorMultiplier, pRange, pMax, whatIsPredator);
            var alignment = new Alignment(alignmentMultiplier);
            var cohesion = new Cohesion(cohesionMultiplier);

            _flocking = new IFlocking[] {predator,alignment,cohesion};
            _boids = new List<IBoid>();
            _colliders = new Collider[maxBoids];
        }

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

            for (int i = 0; i < _flocking.Length; i++)
            {
                var currFlock = _flocking[i];
                dir += currFlock.GetDir(_boids, _self);
            }
            
            return dir.normalized;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, pRange);
        }
    }
}
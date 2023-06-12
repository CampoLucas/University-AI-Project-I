using System;
using System.Collections.Generic;
using Game.Entities.Flocking;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeFlockingManager : FlockingManager
    {
        [Header("Predator")]
        [Range(1,10)][SerializeField] private float pRange;
        [Range(1,10)][SerializeField] private int pMax;
        [SerializeField] private LayerMask whatIsPredator;
        [Space]
        [Header("Multipliers")]
        [Range(0f,10f)][SerializeField] private float predatorMultiplier;
        [Range(0f,10f)][SerializeField] private float alignmentMultiplier;
        [Range(0f,10f)][SerializeField] private float cohesionMultiplier;
        
        protected override void SetFlocking()
        {
            var predator = new Predator(predatorMultiplier, pRange, pMax, whatIsPredator);
            var alignment = new Alignment(alignmentMultiplier);
            var cohesion = new Cohesion(cohesionMultiplier);

            Flocking = new IFlocking[] {predator,alignment,cohesion};
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, pRange);
        }
    }
}
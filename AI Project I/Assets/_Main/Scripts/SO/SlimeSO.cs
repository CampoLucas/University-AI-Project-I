using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "SlimeStats", menuName = "SO/Entities/SlimeStats", order = 3)]
    public class SlimeSO : EnemySO
    {
        public float JumpForce => jumpForce;

        public float JumpDelay => jumpDelay;

        public float FlockingMultiplier => flockingMultiplier;

        public int MaxBoids => maxBoids;

        public LayerMask WhatIsBoid => whatIsBoid;

        public float BoidRadius => boidRadius;

        public float MinSize => minSize;

        public float MaxSize => maxSize;

        public float PredatorRange => predatorRange;

        public int MaxPredators => maxPredators;

        public LayerMask WhatIsPredator => whatIsPredator;
        public float PredatorMultiplier => predatorMultiplier;

        public float AlignmentMultiplier => alignmentMultiplier;

        public float CohesionMultiplier => cohesionMultiplier;

        [Header("Slime Values")]
        [Range(0.1f, 10)][SerializeField] private float jumpForce = 2;
        [Range(0.1f, 10)][SerializeField] private float jumpDelay = 1;
        [Space]
        [Header("Flocking")] 
        [Range(1, 10)] [SerializeField] private float flockingMultiplier = 2f;
        [Space]
        [Header("General")]
        [Range(1, 15)] [SerializeField] private int maxBoids;
        [SerializeField] private LayerMask whatIsBoid;
        [Range(0.1f, 25)][SerializeField] private float boidRadius = 5;
        [SerializeField] private float minSize = 0.5f;
        [SerializeField] private float maxSize = 2f;
        [Header("Predator")]
        [Range(1,10)][SerializeField] private float predatorRange;
        [Range(1,10)][SerializeField] private int maxPredators;
        [SerializeField] private LayerMask whatIsPredator;
        [Header("Multipliers")]
        [Range(0f,10f)][SerializeField] private float predatorMultiplier;
        [Range(0f,10f)][SerializeField] private float alignmentMultiplier;
        [Range(0f,10f)][SerializeField] private float cohesionMultiplier;
    }
}
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "SlimeStats", menuName = "SO/Entities/SlimeStats", order = 3)]
    public class SlimeSO : EnemySO
    {
        public float JumpForce => jumpForce;

        public float JumpDelay => jumpDelay;

        public float GrowSpeed => growSpeed;

        public float RandomPointRadius => randomPointRadius;

        public float RotationMultiplier => rotationMultiplier;

        public int MaxBoids => maxBoids;

        public LayerMask WhatIsBoid => whatIsBoid;

        public float BoidRadius => boidRadius;

        public float PredatorRange => predatorRange;

        public int MaxPredators => maxPredators;

        public LayerMask WhatIsPredator => whatIsPredator;

        public float FlockingMultiplier => flockingMultiplier;

        public float PredatorMultiplier => predatorMultiplier;

        public float AlignmentMultiplier => alignmentMultiplier;

        public float CohesionMultiplier => cohesionMultiplier;

        public float MoveOdds => moveOdds;

        public float IdleOdds => idleOdds;

        public float SpinOdds => spinOdds;

        public float PowerUpOdds => powerUpOdds;

        [Header("Slime Values")]
        [Range(0.1f,10)][SerializeField] private float jumpForce = 2;
        [Range(0.1f,10)][SerializeField] private float jumpDelay = 1;
        [Range(0.1f,2)][SerializeField] private float growSpeed = 1;
        [Range(1f,100f)][SerializeField] private float randomPointRadius = 25;
        [Range(1f,100f)][SerializeField] private float rotationMultiplier = 75;
        [Space]
        [Header("Flocking")]
        [Range(1,15)][SerializeField] private int maxBoids;
        [SerializeField] private LayerMask whatIsBoid;
        [Range(0.1f,25)][SerializeField] private float boidRadius = 5;
        [Header("Predator")]
        [Range(1,10)][SerializeField] private float predatorRange;
        [Range(1,10)][SerializeField] private int maxPredators;
        [SerializeField] private LayerMask whatIsPredator;
        [Space]
        [Header("Multipliers")]
        [Range(1,10)][SerializeField] private float flockingMultiplier = 2f;
        [Range(0f,10f)][SerializeField] private float predatorMultiplier;
        [Range(0f,10f)][SerializeField] private float alignmentMultiplier;
        [Range(0f,10f)][SerializeField] private float cohesionMultiplier;

        [Header("Roulette")]
        [Range(0.1f,100f)][SerializeField]private float moveOdds = 1;
        [Range(0.1f,100f)][SerializeField]private float idleOdds = 1;
        [Range(0.1f,100f)][SerializeField]private float spinOdds = 1;
        [Range(0.1f, 100f)] [SerializeField] private float powerUpOdds = 1;
    }
}
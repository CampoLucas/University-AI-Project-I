using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/Entities/EnemyStats", order = 2)]
    public class EnemySO : StatSO
    {
        public float AttackRange => attackRange;
        public float FovRange => fovRange;
        public float FovAngle => fovAngle;
        public LayerMask FovMask => fovMask;
        public float ObsRange => obsRange;
        public float ObsAngle => obsAngle;
        public int MaxObs => maxObs;
        public float ObsMultiplier => obsMultiplier;
        public LayerMask ObsMask => obsMask;
        public float PursuitTime => pursuitTime;

        [Header("Attack")]
        [SerializeField] private float attackRange = 0.5f;
        
        [Header("Cone Vision")]
        [SerializeField] private float fovRange = 5;
        [SerializeField] private float fovAngle = 120;
        [SerializeField] private LayerMask fovMask;
        
        [Header("Obstacle Avoidance")]
        [SerializeField] private float obsRange = 5;
        [SerializeField] private float obsAngle = 90;
        [SerializeField] private int maxObs = 5;
        [SerializeField] private float obsMultiplier = 1;
        [SerializeField] private LayerMask obsMask;

        [Header("Pursuit")] 
        [SerializeField] private float pursuitTime = 0.5f;
    }
}
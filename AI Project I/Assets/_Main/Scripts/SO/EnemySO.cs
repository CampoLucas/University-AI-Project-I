using Game.Data;
using Game.Enemies;
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/Entities/EnemyStats", order = 2)]
    public class EnemySO : StatSO
    {
        public float AttackRange => attackRange;
        public FieldOfViewData FOV => fieldOfView; 
        public float ObsRange => obsRange;
        public float ObsAngle => obsAngle;
        public int MaxObs => maxObs;
        public float ObsMultiplier => obsMultiplier;
        public LayerMask ObsMask => obsMask;
        public float PursuitTime => pursuitTime;

        [Header("Attack")]
        [SerializeField] private float attackRange = 0.5f;
        
        [Header("Cone Vision")]
        [SerializeField] private FieldOfViewData fieldOfView;
        
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
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Stats", menuName = "SO/Entities/Stats", order = 0)]
    public class StatSO : ScriptableObject
    {
        public float MoveSpeed => moveSpeed;
        public float MoveLerpSpeed => moveLerpSpeed;
        public float RotSpeed => rotSpeed;
        public float MaxHealth => maxHealth;
        public float InvulnerableCooldown => invulnerableCooldown;
        public AnimationEventSO HitAnimation => hitAnimation;
        public AnimationEventSO DeathAnimation => deathAnimation;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float moveLerpSpeed = 10;

        [Header("Rotation")] 
        [SerializeField] private float rotSpeed = 10;

        [Header("Health")] 
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float invulnerableCooldown = 0.1f;
        [SerializeField] private AnimationEventSO hitAnimation;
        [SerializeField] private AnimationEventSO deathAnimation;


    }
}
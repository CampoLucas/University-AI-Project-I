using UnityEngine;

namespace Game.Entities
{
    public class Damageable : MonoBehaviour
    {
        public bool IsInvulnerable { get; private set; }
        private float _currentLife;
        
        /// <summary>
        /// All the damage is rounded in n / n.75 / n.5 / n.25
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(float damage)
        {
            var roundedDamage = Mathf.Round(damage * 4) / 4f;
            _currentLife -= roundedDamage;
        }
        
    }
    
    
}
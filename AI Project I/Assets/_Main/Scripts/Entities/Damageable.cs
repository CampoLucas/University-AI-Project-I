using System;
using System.Collections;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class Damageable : MonoBehaviour
    {
        private StatSO _data;
        private float _currentLife;
        private bool _isInvulnerable;
        private bool _hasTakenDamage;

        private void InitStats()
        {
            _currentLife = _data.MaxHealth;
        }
        private void Awake()
        {
            _data = GetComponent<EntityModel>().GetData();
        }

        private void Start()
        {
            InitStats();
        }

        private void Update()
        {
            if (HasTakenDamage())
                print("ouch");
        }

        private void LateUpdate()
        {
            _hasTakenDamage = false;
        }

        public bool IsAlive() => _currentLife > 0;
        public bool IsInvulnerable() => _isInvulnerable;
        public bool HasTakenDamage() => _hasTakenDamage;
        
        public void TakeDamage(float damage)
        {
            if (IsAlive() && !_isInvulnerable)
            {
                var roundedDamage = Mathf.Round(damage * 4) / 4f;
                _currentLife -= roundedDamage;

                _hasTakenDamage = true;
                TurnInvulnerable();
            }
        }

        private void TurnInvulnerable()
        {
            _isInvulnerable = true;
            StartCoroutine(InvulnerableCooldown());
        }

        private IEnumerator InvulnerableCooldown()
        {
            yield return new WaitForSeconds(_data.InvulnerableCooldown);
            _isInvulnerable = false;
        }
        
        public void Die()
        {
            gameObject.SetActive(false);
        } 
    }
}
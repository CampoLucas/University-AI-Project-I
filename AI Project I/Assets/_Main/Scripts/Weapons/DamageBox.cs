using System.Collections.Generic;
using UnityEngine;
using Game.SO;
using Game.Sheared;
using Game.Entities;

namespace Game.Items.Weapons
{
    public class DamageBox : MonoBehaviour
    {
        private Damageable _damageable;
        private BoxCastTrigger _trigger;
        private WeaponSO _data;
        private Dictionary<GameObject, Damageable> _damageables = new();
        

        public void InitData(WeaponSO data)
        {
            _data = data;
        }

        private void Awake()
        {
            _trigger = GetComponent<BoxCastTrigger>();
            _damageable = GetComponentInParent<Damageable>();
        }

        private void Start()
        {
            _trigger.OnCastEnter += CastEnter;
        }

        private void Damage(Damageable damageable)
        {
            damageable.TakeDamage(_data.Damage);
        }

        private void CastEnter(Collider other)
        {
            var otherGameObject = other.gameObject;
            if (_damageables.TryGetValue(otherGameObject, out var damageable))
            {
                Damage(damageable);
            }
            else
            {
                damageable = otherGameObject.GetComponent<Damageable>();
                if (damageable)
                {
                    _damageables[otherGameObject] = damageable;
                    Damage(damageable);
                }
            }
            
        }

        private void OnDestroy()
        {
            _trigger.OnCastEnter -= CastEnter;
            _trigger = null;
            _data = null;
            _damageables = null;
        }
    }
}

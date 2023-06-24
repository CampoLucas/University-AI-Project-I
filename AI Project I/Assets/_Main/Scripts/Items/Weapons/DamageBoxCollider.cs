using System;
using System.Collections.Generic;
using UnityEngine;
using Game.SO;
using Game.Sheared;
using Game.Entities;

namespace Game.Items.Weapons
{
    public class DamageBoxCollider : MonoBehaviour
    {
        [SerializeField] private bool destroyOnEnter;
        [SerializeField] private float damage;
        private Damageable _damageable;
        private WeaponSO _data;
        private Dictionary<GameObject, Damageable> _damageables = new();
        

        public void InitData(WeaponSO data)
        {
            _data = data;
        }

        private void Awake()
        {
            _damageable = GetComponentInParent<Damageable>();
        }
        private void Damage(Damageable damageable)
        {
            damageable.TakeDamage(damage);
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     var otherGameObject = other.gameObject;
        //     if (_damageables.TryGetValue(otherGameObject, out var damageable))
        //     {
        //         Damage(damageable);
        //     }
        //     else
        //     {
        //         damageable = otherGameObject.GetComponent<Damageable>();
        //         if (damageable)
        //         {
        //             _damageables[otherGameObject] = damageable;
        //             Damage(damageable);
        //         }
        //     }
        //     if (destroyOnEnter) Destroy(this);
        // }

        private void OnCollisionEnter(Collision other)
        {
            var otherGameObject = other.gameObject;
            if (_damageables.TryGetValue(otherGameObject, out var damageable))
            {
                if (_damageable && damageable == _damageable) return;
                Damage(damageable);
            }
            else
            {
                damageable = otherGameObject.GetComponent<Damageable>();
                if (damageable)
                {
                    _damageables[otherGameObject] = damageable;
                    if (_damageable && damageable == _damageable) return;
                    Damage(damageable);
                }
            }
            if (destroyOnEnter) Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _data = null;
            _damageables = null;
        }
    }
}
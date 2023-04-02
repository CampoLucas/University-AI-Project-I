using System;
using UnityEngine;
using Game.SO;
using Game.Sheared;

namespace Game.Items.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponSO stats;
        private BoxCastTrigger _trigger;
        private DamageBox _damageBox;

        private void InitData()
        {
            _damageBox.InitData(stats);
            // _trigger.InitData(stats)
        }

        private void Awake()
        {
            _trigger = GetComponentInChildren<BoxCastTrigger>();
            _damageBox = GetComponentInChildren<DamageBox>();
            InitData();
        }

        public WeaponSO GetData()
        {
            return stats;
        }

        public void EnableTrigger()
        {
            if (_trigger)
            {
                _trigger.EnableCollider();
            }
        }

        public void DisableTrigger()
        {
            if (_trigger)
            {
                _trigger.DisableCollider();
            }
        }
    }
}
using System;
using UnityEngine;
using Game.SO;
using Game.Sheared;
using UnityEngine.Serialization;

namespace Game.Items.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [FormerlySerializedAs("stats")] [SerializeField] protected WeaponSO Stats;
        private BoxCastTrigger _trigger;
        private DamageBox _damageBox;

        private void InitData()
        {
            if (_damageBox)
                _damageBox.InitData(Stats);
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
            return Stats;
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
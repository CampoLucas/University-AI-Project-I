using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class HeavyAttack : MonoBehaviour, IAttack
    {
        private EntityModel _entity;
        private float _timer;
        private bool _enable;
        private bool _activated;
        private bool _deactivated;

        private void Awake()
        {
            _entity = GetComponent<EntityModel>();
        }

        private void Update()
        {
            if (_enable)
            {
                _timer += Time.deltaTime;

                if (!_activated && _timer >= _entity.CurrentWeapon().GetData().HeavyAttackTriggerStarts)
                {
                    _activated = true;
                    _entity.CurrentWeapon().EnableTrigger();
                }
                if (!_deactivated && _timer >= _entity.CurrentWeapon().GetData().HeavyAttackTriggerEnds)
                {
                    _deactivated = false;
                    _entity.CurrentWeapon().DisableTrigger();
                    _enable = false;
                }
            }
        }

        public void Attack()
        {
            _enable = true;
            _activated = false;
            _deactivated = false;
            _timer = 0;
        }
    }
}

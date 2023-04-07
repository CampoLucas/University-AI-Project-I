using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class LightAttack : MonoBehaviour, IAttack
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

        private void FixedUpdate()
        {
            if (_enable)
            {
                _timer += Time.fixedDeltaTime;

                if (!_activated && _timer >= _entity.CurrentWeapon().GetData().LightAttackTriggerStarts)
                {
                    _activated = true;
                    _entity.CurrentWeapon().EnableTrigger();
                }
                if (!_deactivated && _timer >= _entity.CurrentWeapon().GetData().LightAttackTriggerEnds)
                {
                    _deactivated = false;
                    _entity.CurrentWeapon().DisableTrigger();
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

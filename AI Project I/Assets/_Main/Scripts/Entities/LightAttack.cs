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
        protected EntityModel Entity;
        private float _timer;
        private bool _enable;
        private bool _activated;
        private bool _deactivated;

        private void Awake()
        {
            Entity = GetComponent<EntityModel>();
        }

        private void Update()
        {
            if (_enable)
            {
                _timer += Time.deltaTime;

                if (!_activated && _timer >= GetStartTime())
                {
                    _activated = true;
                    Entity.CurrentWeapon().EnableTrigger();
                }
                if (!_deactivated && _timer >= GetEndTime())
                {
                    _deactivated = false;
                    Entity.CurrentWeapon().DisableTrigger();
                    _enable = false;
                }
            }
        }

        protected virtual float GetStartTime() => Entity.CurrentWeapon().GetData().LightAttackTriggerStarts;
        protected virtual float GetEndTime() => Entity.CurrentWeapon().GetData().LightAttackTriggerEnds;

        public void Attack()
        {
            _enable = true;
            _activated = false;
            _deactivated = false;
            _timer = 0;
        }

        public void CancelAttack()
        {
            _enable = false;
            Entity.CurrentWeapon().DisableTrigger();
        }
    }
}

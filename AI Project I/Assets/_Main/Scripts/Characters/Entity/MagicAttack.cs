using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class MagicAttack : MonoBehaviour, IAttack
    {
        protected EntityModel Entity;
        private float _timer;
        private bool _enable;
        private bool _activated;

        private void Awake()
        {
            Entity = GetComponent<EntityModel>();
        }

        protected void Update()
        {
            if (_enable)
            {
                _timer += Time.deltaTime;

                if (_timer >= GetStartTime())
                {
                    _activated = true;
                    var staff = Entity.CurrentWeapon() as RangeWeapon;
                    staff.FireProjectile(transform.forward);
                    _enable = false;
                }
            }
        }

        protected virtual float GetStartTime() => Entity.CurrentWeapon().GetData().LightAttackTriggerStarts;
        protected virtual float GetEndTime() => Entity.CurrentWeapon().GetData().LightAttackTriggerEnds;

        public void Attack()
        {
            _enable = true;
            _timer = 0;
        }

        public void CancelAttack()
        {
            _enable = false;
            Entity.CurrentWeapon().DisableTrigger();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            Entity = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class LightAttack : MonoBehaviour, IAttack
    {
        public float aTime = 0.18f;
        public float dTime = 0.4f;
        public void Attack(Weapon weapon)
        {
            StartCoroutine(AttackCoroutine(weapon));
        }

        private IEnumerator AttackCoroutine(Weapon weapon)
        {
            yield return new WaitForSeconds(aTime);
            weapon.EnableTrigger();
            yield return new WaitForSeconds(dTime);
            weapon.DisableTrigger();
        }
    }
}

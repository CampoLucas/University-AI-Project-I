using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class HeavyAttack : MonoBehaviour, IAttack
    {
        public void Attack(Weapon weapon, View anim)
        {
            anim.PlayTargetAnimation(weapon.GetData().HeavyAttack01, true);
        }
    }
}

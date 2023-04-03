using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class LightAttack : MonoBehaviour, IAttack
    {
        public void Attack(Weapon weapon, EntityView anim)
        {
            anim.PlayTargetAnimation(weapon.GetData().LightAttack01, true);
        }
    }
}

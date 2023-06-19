using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class HeavyAttack : LightAttack
    {
        protected override float GetStartTime() => Entity.CurrentWeapon().GetData().HeavyAttackTriggerStarts;
        protected override float GetEndTime() => Entity.CurrentWeapon().GetData().HeavyAttackTriggerEnds;
    }
}

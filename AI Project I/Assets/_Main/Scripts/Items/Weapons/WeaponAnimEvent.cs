using System.ComponentModel;
using System;
using System.Reflection;

namespace Game.Entities.Weapons
{
    public enum WeaponAnimEvent
    {
        [Description("Sword Idle 01")]
        SwordHandIdle,
        [Description("Sword Light Attack 01")]
        SwordLightAttack01,
        [Description("Sword Light Attack 02")]
        SwordLightAttack02,
        [Description("Sword Heavy Attack 01")]
        SwordHeavyAttack,
    }
}
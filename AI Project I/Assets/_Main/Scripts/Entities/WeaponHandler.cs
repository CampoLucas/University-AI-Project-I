using System.Collections;
using System.Collections.Generic;
using Game.Items.Weapons;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;

    public void EnableRightWeaponDamageTrigger()
    {
        currentWeapon.EnableTrigger();
    }
    
    public void DisableRightWeaponDamageTrigger()
    {
        currentWeapon.DisableTrigger();
    }
}

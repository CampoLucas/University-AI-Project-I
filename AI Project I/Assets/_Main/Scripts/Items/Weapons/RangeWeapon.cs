using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Items;
using Game.Items.Weapons;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    public void FireProjectile(Vector3 dir)
    {
        LoggingTwo.Log("Projectile spawned");
        
        var p = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        p.transform.rotation = Quaternion.LookRotation(dir);
        p.InitData(Stats);
    }
}

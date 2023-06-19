using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{

    [SerializeField] private GameObject spawner;
    private BulletShooter _bulletshooter;
    
    private void Start()
    {
        _bulletshooter = spawner.GetComponent<BulletShooter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _bulletshooter.ShootBullet();
    }
}

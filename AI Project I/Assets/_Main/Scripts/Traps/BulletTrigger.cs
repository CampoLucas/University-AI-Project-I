using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{

    [SerializeField] private GameObject spawner;
    
    private void OnTriggerEnter(Collider other)
    {
        spawner.GetComponent<BulletShooter>().ShootBullet();
    }
}

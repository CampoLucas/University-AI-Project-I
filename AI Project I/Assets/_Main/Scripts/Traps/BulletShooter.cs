using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    
    [SerializeField] private GameObject bullet;

    public void ShootBullet()
    {
        Instantiate(bullet,this.transform.position, this.transform.rotation, this.transform);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletShooter : MonoBehaviour
{
    
    [SerializeField] private BulletScript _bullet;
    private ObjectPool<BulletScript> poolOfbullets;

    private void Start()
    {
        poolOfbullets = new ObjectPool<BulletScript>(CreateBullet, OnGetBullet, 
            OnReleaseBullet, OnDestroyBullet);
    }

    public void ShootBullet()
    {
        poolOfbullets.Get();
    }

    public BulletScript CreateBullet()
    {
        BulletScript bullet = Instantiate(_bullet,this.transform.position, this.transform.rotation, this.transform);
        bullet.SetPool(poolOfbullets);
        return bullet;
    }

    public void OnGetBullet(BulletScript bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
    }

    protected void OnReleaseBullet(BulletScript bullet)
    {
        bullet.Deactivate();
    }

    protected void OnDestroyBullet(BulletScript bullet)
    {
        Destroy(bullet);
    }
}

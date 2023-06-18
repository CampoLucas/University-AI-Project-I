using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;
using UnityEngine.Pool;

public class BulletScript : MonoBehaviour
{
    bool isActive;
    private GameObject Bullet;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    public String Owner;
    private IObjectPool<BulletScript> mypool;

    void Start()
    {
        isActive = true;
        Bullet = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        isActive = true;                
    }

    public void Deactivate()
    {
        isActive = false;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable player = other.GetComponent<Damageable>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        mypool.Release(this);
    }

    public void SetPool(IObjectPool<BulletScript> pool)
    {
        mypool = pool;
    }
}
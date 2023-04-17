using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    bool isActive;
    private GameObject Bullet;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    public String Owner;

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

    public void Activate()
    {
        isActive = true;        
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (!other.gameObject.CompareTag("Untagged"))
        {
            Damageable player = other.GetComponent<Damageable>();
            Debug.Log("Rompe Bala");
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(this);
            
        }
    }
}
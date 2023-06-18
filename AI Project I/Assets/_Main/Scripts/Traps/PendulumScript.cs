using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float limit;
    [SerializeField] private float damage;
    private float random;
    private void Awake()
    {
            random = Random.Range(0, 1);
    }

    void Update()
    {
        float angle = limit * Mathf.Cos(Time.time + random * speed);
        transform.localRotation = Quaternion.Euler(0,0,- angle);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Damageable player = other.GetComponent<Damageable>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}

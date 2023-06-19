using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

public class PunjiStick : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
        {
            Damageable player = other.GetComponent<Damageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    
}
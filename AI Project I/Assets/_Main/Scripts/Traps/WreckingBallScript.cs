using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

public class WreckingBallScript : MonoBehaviour
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

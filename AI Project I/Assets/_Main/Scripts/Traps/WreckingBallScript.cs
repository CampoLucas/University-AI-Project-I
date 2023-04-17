using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;

public class WreckingBallScript : MonoBehaviour
{
    [SerializeField] private float damage;


    private void OnCollisionEnter(Collision collision)
    {
        Damageable player = collision.body.GetComponent<Damageable>();
        if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
}

using System;
using Game.Entities;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Damageable player;

        private void Start()
        {
            player.OnTakeDamage += OnTakeDamageHandler;
        }

        private void OnTakeDamageHandler()
        {
            var currentLife = player.CurrentLife;
            
            container.transform.GetChild((int)currentLife).gameObject.SetActive(false);
        }
    }
}
using System;
using Game.Entities;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Damageable player;
        [SerializeField] private GameObject lifeSprite;

        private void Start()
        {
            player.OnTakeDamage += OnTakeDamageHandler;

            var lives = player.CurrentLife;

            for (int i = 0; i < lives; i++)
            {
                Instantiate(lifeSprite, container.transform);
            }
        }

        private void OnTakeDamageHandler()
        {
            var currentLife = player.CurrentLife;
            
            container.transform.GetChild((int)currentLife).gameObject.SetActive(false);
        }
    }
}
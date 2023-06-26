using System;
using System.Globalization;
using Game.Entities;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private Damageable enemy;
        
        private void Start()
        {
            enemy.OnTakeDamage += OnTakeDamageHandler;
            
            UpdateText();
        }

        private void OnTakeDamageHandler()
        {
            UpdateText();
            if (enemy.CurrentLife == 0)
            {
                canvas.alpha = 0;
            }

        }

        private void UpdateText()
        {
            text.text = enemy.CurrentLife.ToString(CultureInfo.InvariantCulture);
        }
    }
}
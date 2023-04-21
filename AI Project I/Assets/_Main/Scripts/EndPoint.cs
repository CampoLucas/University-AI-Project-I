using System;
using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class EndPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = GetComponent<PlayerModel>();
            if (player && GameManager.Instance != null)
            {
                GameManager.Instance.GameWon();
            }
        }
    } 
}

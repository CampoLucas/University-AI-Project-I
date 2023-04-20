using System;
using Game.Managers;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class Window : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.SetCanvasTransform(transform);
        }
    }
}
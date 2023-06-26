using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace Game.Scripts.UI
{
    public class MessageScreen : MonoBehaviour
    {
        [SerializeField] private float delayTime = 1f;
        private float _currentTime;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _canvasGroup.alpha = 1;
            _currentTime = delayTime;
        }

        private void Update()
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime < 0)
            {
                _canvasGroup.alpha = 0;
            }
            else if(_currentTime < 0.5f)
                Destroy(gameObject);
            
        }
    }
}
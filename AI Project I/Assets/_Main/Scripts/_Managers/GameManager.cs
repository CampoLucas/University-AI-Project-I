using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameOverScreen gameOverPrefab;
        [SerializeField] private GameOverScreen gameWonPrefab;
        private GameOverScreen _gameOverScreen;
        private GameOverScreen _gameWonScreen;
        private Transform _canvas;

        private static GameManager _instance = null;
        private static readonly object Padlock = new();

        public static GameManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance;
                }
            }
        }

        private void Awake()
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = this;
                }
            }
        }

        public void InitScreens()
        {
            _gameOverScreen = Instantiate(gameOverPrefab, _canvas);
            _gameOverScreen.gameObject.SetActive(false);
            _gameWonScreen = Instantiate(gameWonPrefab, _canvas);
            _gameWonScreen.gameObject.SetActive(false);
        }

        public void SetCanvasTransform(Transform canvas)
        {
            _canvas = canvas;
            InitScreens();
        }

        public void GameOver()
        {
            _gameOverScreen.gameObject.SetActive(true);
            _gameOverScreen.Init();
        }
        
        public void GameWon()
        {
            _gameWonScreen.gameObject.SetActive(true);
            _gameWonScreen.Init();
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace Game.Scripts.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private float delayTime = 1f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;
        private CanvasGroup _canvasGroup;

        public void Init()
        {
            _canvasGroup.alpha = 0;
            StartCoroutine(FadeInCanvasGroup());
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            retryButton.onClick.AddListener(ResetLevel);
            mainMenuButton.onClick.AddListener(MainMenu);
        }

        private void OnDestroy()
        {
            retryButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
        }

        private void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        
        private IEnumerator FadeInCanvasGroup()
        {
            var elapsedTime = 0f;
            var startAlpha = _canvasGroup.alpha;
            var endAlpha = 1f;

            yield return new WaitForSeconds(delayTime);
            while (elapsedTime < fadeDuration)
            {
                var t = elapsedTime / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = endAlpha;
        }
    }
}
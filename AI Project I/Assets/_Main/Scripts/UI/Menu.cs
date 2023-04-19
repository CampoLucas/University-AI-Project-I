using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Methods that all type of menu have in common
/// </summary>
public class Menu : BaseComponent
{
    /// <summary>
    /// Hides the menu
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Shows the menu
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // private IEnumerator Fade(float targetNumber, float fadeDuration)
    // {
    //     var initNumber = canvasGroup.alpha;
    //     var elapsedTime = 0f;
    //
    //     while (elapsedTime < fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         canvasGroup.alpha = Mathf.Lerp(initNumber, targetNumber, elapsedTime / fadeDuration);
    //         yield return null;
    //     }
    // }
}

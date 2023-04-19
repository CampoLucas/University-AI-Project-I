using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that notifies a mediator when a button is pressed, has specific variables for the Options Menu
/// </summary>
public class OptionsMenu : Menu
{
    [SerializeField] private Button backButton;
    private void Start()
    {
        var mediator = Mediator as MenuMediator;
        if (!mediator) return;
        backButton.onClick.AddListener(() => mediator.Notify(this, "BACK"));
    }
    
    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        var mediator = Mediator as MenuMediator;
        if (!mediator) return;
        backButton.onClick.RemoveAllListeners();
    }
}

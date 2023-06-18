using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that notifies a mediator when a button is pressed, has specific variables for the Main Menu
/// </summary>
public class MainMenu : Menu
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        var mediator = Mediator as MenuMediator;
        if (!mediator) return;
        playButton.onClick.AddListener(() => mediator.Notify(this, "PLAY"));
        optionsButton.onClick.AddListener(() => mediator.Notify(this, "OPTIONS"));
        quitButton.onClick.AddListener(() => mediator.Notify(this, "QUIT"));
    }

    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        var mediator = Mediator as MenuMediator;
        if (!mediator) return;
        playButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }
}

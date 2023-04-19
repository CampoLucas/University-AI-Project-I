using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Mediator class that handles the different menus in the Main Menu
/// </summary>
public class MenuMediator : MonoBehaviour, IMediator
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private OptionsMenu optionsMenu;

    private void Start()
    {
        mainMenu.SetMediator(this);
        optionsMenu.SetMediator(this);
        optionsMenu.Hide();
    }

    private void Play() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    private void Options()
    {
        mainMenu.Hide();
        optionsMenu.Show();
    }

    private void Quit() => Application.Quit();

    private void Back()
    {
        optionsMenu.Hide();
        mainMenu.Show();
    }

    public void Notify(object sender, string ev)
    {
        switch (ev)
        {
            case "PLAY":
                Play();
                return;
            case "OPTIONS":
                Options();
                return;
            case "QUIT":
                Quit();
                return;
            case "BACK":
                Back();
                return;
            default:
                return;
        }
    }
}

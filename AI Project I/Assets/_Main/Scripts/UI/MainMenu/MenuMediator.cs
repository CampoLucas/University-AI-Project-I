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
    [SerializeField] private CreditsMenu creditsMenu;

    private void Awake()
    {
        mainMenu.SetMediator(this);
        creditsMenu.SetMediator(this);
        creditsMenu.Hide();
    }

    private void Play() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    private void Credits()
    {
        mainMenu.Hide();
        creditsMenu.Show();
    }

    private void Quit() => Application.Quit();

    private void Back()
    {
        creditsMenu.Hide();
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
                Credits();
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

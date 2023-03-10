using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(0);
        mostraCursore();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        mostraCursore();
    }

    public void LoadHowToPlay()
    {
        SceneManager.LoadScene(2);
        mostraCursore();
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene(3);
        mostraCursore();
    }

    public void LoadLobbies()
    {
        SceneManager.LoadScene(4);
        mostraCursore();
    }

    public void LoadCreateLobby()
    {
        SceneManager.LoadScene(5);
        mostraCursore();
    }

    private void mostraCursore() // mostra il cursore per poter interagire con i bottoni
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
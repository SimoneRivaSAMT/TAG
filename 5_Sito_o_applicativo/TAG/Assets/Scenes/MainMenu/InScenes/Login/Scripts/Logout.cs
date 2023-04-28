using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scenes;

public class Logout : MonoBehaviour
{
    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene((int)SceneToId.mainMenu);
    }
}

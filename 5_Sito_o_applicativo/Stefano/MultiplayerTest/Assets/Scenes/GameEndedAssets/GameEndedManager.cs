using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/*
 Quando la connessione crasha
 */

public class GameEndedManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        scoreText.text = "Score: " + PlayerPrefs.GetInt("score");
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

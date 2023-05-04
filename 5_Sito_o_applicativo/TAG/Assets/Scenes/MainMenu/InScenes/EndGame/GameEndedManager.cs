using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/*
 Schermata di fine gioco
 */

public class GameEndedManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        scoreText.text = "Your score: " + PlayerPrefs.GetInt("last_score");
        if(PlayerPrefs.GetInt("is_top_scorer") == 1)
        {
            Debug.LogError("TOP SCORER");
        }
        PlayerPrefs.DeleteKey("is_top_scorer");
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackManager : MonoBehaviour
{
    private InputManager inputManager;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.ui.Back.triggered)
        {
            SceneManager.LoadScene(id);
        }
    }

    public void SetInt(int id)
    {
        this.id = id;
    }
}

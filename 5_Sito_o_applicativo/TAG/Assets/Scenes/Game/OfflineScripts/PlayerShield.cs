using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject shield;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.onAction.Shield.IsPressed())
        {
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
        }
    }
}

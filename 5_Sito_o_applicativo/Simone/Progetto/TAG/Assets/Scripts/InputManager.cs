using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.UIActions UI;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        UI = playerInput.UI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        UI.Enable();
    }

    private void OnDisable()
    {
        UI.Disable();
    }

    
}

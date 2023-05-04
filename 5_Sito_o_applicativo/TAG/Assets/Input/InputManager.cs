using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Assets.Scenes;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.UIActions ui;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.OnActionActions onAction;
    public bool HasPlayerGivenStartCommand { get; private set; }

    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        ui = playerInput.UI;
        onFoot = playerInput.OnFoot;
        onAction = playerInput.OnAction;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneToId.offlineGame
            || SceneManager.GetActiveScene().buildIndex == (int)SceneToId.onlineGame)
        {
            onFoot.Jump.performed += ctx => motor.Jump();
            onFoot.Crouch.performed += ctx => motor.Crouch();
            onAction.StartMatch.performed += ctx => FindObjectOfType<NetworkMatchManager>().StartMatch();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneToId.offlineGame
            || SceneManager.GetActiveScene().buildIndex == (int)SceneToId.onlineGame)
        {
            // Tell the PlayerMotor to move using the value from our movement action
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneToId.offlineGame
            || SceneManager.GetActiveScene().buildIndex == (int)SceneToId.onlineGame)
        {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>(), false);
            look.ProcessLook(onFoot.LookGamepad.ReadValue<Vector2>(), true);
        }
    }

    private void OnEnable()
    {
        ui.Enable();
        onFoot.Enable();
        onAction.Enable();
    }

    private void OnDisable()
    {
        ui.Disable();
        onFoot.Disable();
        onAction.Disable();
    }
}

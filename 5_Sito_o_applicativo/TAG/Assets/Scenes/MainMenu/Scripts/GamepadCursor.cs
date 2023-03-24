using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private RectTransform cursorTransform;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform canvasTransform;

    [SerializeField]
    private float cursorSpeed = 1000f;

    [SerializeField]
    private float padding = 50f;


    private bool previousMouseState;
    private Camera mainCamera;
    private string previousControlScheme = "";
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Vector2 newPosition = new Vector2(0,0);

    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    private void Awake() {
        Cursor.SetCursor(default, newPosition, CursorMode.ForceSoftware);
        Cursor.visible = false;
        Debug.Log("wee");
    }

    private void Update() {
        Cursor.SetCursor(default, newPosition, CursorMode.ForceSoftware);
    }

    private void OnEnable(){
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        InputDevice virtualMouseInputDevice = InputSystem.GetDevice("VirtualMouse");

        if(virtualMouseInputDevice == null){
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if(!virtualMouseInputDevice.added){
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }else{
            virtualMouse = (Mouse)virtualMouseInputDevice;
        }

        InputUser user = default;
        InputUser.PerformPairingWithDevice(virtualMouse, user);

        if(cursorTransform != null){
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        //playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable() {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        //playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion(){
        if(virtualMouse == null || Gamepad.current == null){
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if(previousMouseState != aButtonIsPressed){
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    /*private void OnControlsChanged(PlayerInput input){
        if(playerInput.c == mouseScheme && previousControlScheme != mouseScheme){
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }else if(playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme){
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
    }*/
}

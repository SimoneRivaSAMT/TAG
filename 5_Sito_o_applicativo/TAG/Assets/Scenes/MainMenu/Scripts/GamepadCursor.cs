using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;
using System.Runtime.InteropServices;

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

    [SerializeField]
    private GameObject cursorObject;


    private PlayerInput player;
    private bool previousMouseState;
    private Camera mainCamera;
    private string previousControlScheme = "";
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Vector2 newPosition = new Vector2(0, 0);
    private UIButtonManager uiButton;
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    private bool status = false;

    private void Awake()
    {
        newPosition = new Vector2(Screen.width/2, Screen.height/2);
        Cursor.SetCursor(default, newPosition, CursorMode.ForceSoftware);
        Cursor.visible = false;
        uiButton = GetComponent<UIButtonManager>();
    }

    private void Start() {
        InputState.Change(virtualMouse.position, newPosition);
    }    

    private void Update()
    {
        Cursor.SetCursor(default, newPosition, CursorMode.ForceSoftware);
        CheckLastInput();
    }

    //Controllo se l'ultimo input è del controller o mouse/tastiera
    private void CheckLastInput(){
        if(Gamepad.current != null){
            Cursor.visible = false;
            cursorObject.SetActive(true);
        }else{
            Cursor.visible = true;
            cursorObject.SetActive(false);
        }
    }

    //Imposto il controller come un mouse virtuale
    private void OnEnable()
    {        
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        InputDevice virtualMouseInputDevice = InputSystem.GetDevice("VirtualMouse");

        if (virtualMouseInputDevice == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouseInputDevice.added)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else
        {
            virtualMouse = (Mouse)virtualMouseInputDevice;
        }

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    //Aggiorno la posizione del cursore del controller
    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        Vector2 currentPosition = virtualMouse.position.ReadValue();

        if(status){
            
            deltaValue *= cursorSpeed * Time.deltaTime;

            
            newPosition = currentPosition + deltaValue;

        
            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);
        }
        

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
        status = true;
    }

    //Ancoro il cursore alla posizione data
    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, position, canvas.renderMode
            == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }


}

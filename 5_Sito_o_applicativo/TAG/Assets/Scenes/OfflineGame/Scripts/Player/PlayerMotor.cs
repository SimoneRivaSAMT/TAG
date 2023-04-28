using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private InputManager inputManager;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch, crouching;
    private float crouchTimer = 0.5f;

    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float crouchTime = 0.1f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        // For Crouching
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / crouchTime;
            p *= p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        // For Sprinting
        if (inputManager.onFoot.Sprint.IsPressed() && !crouching)
        {
            speed = 8;
        }
        else
        {
            speed = 5;
            if (crouching)
                speed = 3;
        }
    }

    // Receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
}
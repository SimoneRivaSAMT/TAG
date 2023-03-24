using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private float xRotation = 0f;
    private float xSensitivity;
    private float ySensitivity;

    public Camera cam;
    
    public float defaultXSensitivity = 30f;
    public float defaultYSensitivity = 30f;
    
    public void ProcessLook(Vector2 input, bool isGamepad) 
    {
        if (isGamepad)
        {
            xSensitivity = defaultXSensitivity * 10;
            ySensitivity = defaultYSensitivity * 10;
        }
        else
        {
            xSensitivity = defaultXSensitivity;
            ySensitivity = defaultYSensitivity;
        }
        

        float mouseX = input.x;
        float mouseY = input.y;
        // Calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply this to our camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}

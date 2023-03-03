using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTest : MonoBehaviour
{
    private InputManager inputManager;
    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }
    public CameraShake cameraShake;
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));
        }*/
    }
}

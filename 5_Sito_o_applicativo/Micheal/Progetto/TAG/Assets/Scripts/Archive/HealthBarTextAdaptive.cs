using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarTextAdaptive : MonoBehaviour
{
    private Transform mainCamera;
    private Transform unit;
    public Transform worldSpaceCanvas;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
        unit = transform.parent;

        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position); // Look at the camera
        transform.position = unit.position + offset;
    }
}

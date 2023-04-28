using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text = "";
        cam = GetComponent<PlayerLook>().cam;
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Eliminate the text not looking
        if(transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text.Equals("Use Keypad"))
        transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text = "";
        // Create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // Variable to store our collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text = interactable.promptMessage;
                if (inputManager.onAction.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}

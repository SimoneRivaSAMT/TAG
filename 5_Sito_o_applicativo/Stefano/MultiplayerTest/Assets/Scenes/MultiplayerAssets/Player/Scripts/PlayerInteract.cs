using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInteract : NetworkBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    private DamageManager damageManager;

    void Start()
    {
        damageManager = GetComponent<DamageManager>();
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;   
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerShoot();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.LogError(GetComponent<NetworkPlayer>().NetworkId);
        }
    }

    public void PlayerShoot()
    {
        // Create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // Variable to store our collision information
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                damageManager.PlayerHittedServerRpc(hitInfo.collider.gameObject.GetComponent<NetworkPlayer>().NetworkId);
                //TODO fix this
            }
        }

    }
}

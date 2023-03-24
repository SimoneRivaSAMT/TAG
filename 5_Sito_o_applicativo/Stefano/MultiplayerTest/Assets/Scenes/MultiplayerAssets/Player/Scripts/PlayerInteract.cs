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

    private DamageManager damageManager;

    void Start()
    {
        damageManager = GetComponent<DamageManager>();
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;   
    }

    void Update()
    {
        if (!IsOwner)
            return;
        playerUI.UpdateText(string.Empty);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerShoot();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (IsHost)
            {
                foreach (KeyValuePair<ulong, GameObject> kvp in FindObjectOfType<PlayersManagement>().GetPlayers())
                {
                    Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
                }
            }
                
            Debug.LogError("My ID (stored locally): "  + GetComponent<NetworkObject>().NetworkObjectId);
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
                ulong net = hitInfo.collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                ulong myNetId = GetComponent<NetworkObject>().NetworkObjectId;
                damageManager.PlayerHittedServerRpc(net, myNetId);
                Debug.LogError("Ho colpito il player " + net);
            }
        }

    }
}

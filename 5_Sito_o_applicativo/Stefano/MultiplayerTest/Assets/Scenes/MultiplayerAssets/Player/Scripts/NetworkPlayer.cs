using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    private Camera _camera;
    private AudioListener _audioListener;
    private PlayersManagement _playersManagement;
    private bool _canQuit = false;

    public LayerMask layerMask;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _audioListener = GetComponentInChildren<AudioListener>();
        _playersManagement = FindObjectOfType<PlayersManagement>();
        if (!IsOwner)
        {
            _audioListener.enabled = false;
            _camera.enabled = false;
            _playersManagement.enabled = false;
        }
        _playersManagement.ClientConnectedServerRpc(GetComponent<NetworkObject>().NetworkObjectId);
    }

    private void Update()
    {
        if (!IsOwner) return;
    }

    #region to-implement-later
    private void HandleShooting()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10000, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Hitted " + hit.collider.GetComponent<NetworkObject>().NetworkObjectId);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100000, Color.yellow);
                Debug.Log("Not Hitted");
            }
        }
    }
    #endregion
    private void Disconnect(ulong instanceId)
    {
        _playersManagement.ClientDisconnectedServerRpc(instanceId);
    }

    public override void OnNetworkDespawn()
    {
        Disconnect(GetComponent<NetworkObject>().NetworkObjectId);
        base.OnNetworkDespawn();
    }
}

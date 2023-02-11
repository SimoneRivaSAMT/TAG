using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerManager : NetworkBehaviour
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
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        float speed = 3f;
        transform.position += moveDir * speed * Time.deltaTime;
    }
    private void HandleShooting()
    {
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10000, layerMask))
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

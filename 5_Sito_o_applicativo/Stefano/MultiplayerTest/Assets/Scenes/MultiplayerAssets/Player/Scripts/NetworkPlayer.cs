using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Assets.GameManagement;

public class NetworkPlayer : NetworkBehaviour
{
    private Camera _camera;
    private AudioListener _audioListener;
    private PlayersManagement _playersManagement;
    private GameManager gameManager;
    private int _life;
    public ulong NetworkId;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _life = 3;
        _camera = GetComponentInChildren<Camera>();
        _audioListener = GetComponentInChildren<AudioListener>();
        _playersManagement = FindObjectOfType<PlayersManagement>();
        if (!IsOwner)
        {
            _audioListener.enabled = false;
            _camera.enabled = false;
            _playersManagement.enabled = false;
        }
        NetworkId = GetComponent<NetworkObject>().NetworkObjectId;
        _playersManagement.ClientConnectedServerRpc(NetworkId);
    }

    private void Update()
    {
        if (!IsOwner) return;
        
    }

    public void TakeDamage()
    {
        _life--;
        print("HITTED! " + _life);
        if(_life == 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _life = 3;
        GetComponent<Transform>().position = gameManager.playerRespawnLocation.position;
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

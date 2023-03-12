using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Assets.GameManagement;
using System;
using UnityEngine.SceneManagement;

public class NetworkPlayer : NetworkBehaviour
{
    private bool serverCrashed = false;
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

            GetComponent<NetworkObject>().enabled = false;
            GetComponent<NetworkPlayer>().enabled = false;
            GetComponent<DamageManager>().enabled = false;
            GetComponent<PlayerInteract>().enabled = false;
            return;
        }
        NetworkId = GetComponent<NetworkObject>().NetworkObjectId;
        _playersManagement.ClientConnectedServerRpc(NetworkId);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (IsHost && NetworkManager.Singleton.ShutdownInProgress)
        {
            Debug.LogError("Singleton shutdown in corso (server)");
            DisconnectAllClients();
        }
        if (Input.GetKey(KeyCode.T)) //match end example
        {
            if (IsHost)
            {
                Debug.Log("Match terminato");
                serverCrashed = false;
                DisconnectAllClients(true);
            }
        }
    }

    private void DisconnectAllClients(bool isMatchEnded = false)
    {
        if(!isMatchEnded)
            serverCrashed = true;
        if (!IsHost)
            return;
        foreach (KeyValuePair<ulong, GameObject> player in _playersManagement.GetPlayers())
        {
            if (player.Key == 1)
                continue;
            player.Value.GetComponent<NetworkObject>().Despawn();
        }
    }

    private void Disconnect(ulong instanceId)
    {
        _playersManagement.ClientDisconnectedServerRpc(instanceId, serverCrashed);
    }

    public override void OnNetworkDespawn()
    {
        Debug.LogError("Player " + GetComponent<NetworkObject>().NetworkObjectId + " despawnato");
        Disconnect(GetComponent<NetworkObject>().NetworkObjectId);
        base.OnNetworkDespawn();
        NetworkManager.Singleton.Shutdown();
        if (!serverCrashed)
        {
            
            SceneManager.LoadScene(0);
        }
        else
        {
            
            SceneManager.LoadScene(2);
        }
        
    }

}

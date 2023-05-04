using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Assets.GameManagement;
using System;
using UnityEngine.SceneManagement;

/*
 Questo script gestisce il player nella rete Netcode
 */

public class NetworkPlayer : NetworkBehaviour
{
    private Camera _camera;
    private AudioListener _audioListener;
    private PlayersManagement _playersManagement;
    private float deltaTime;
    public ulong NetworkId;
    public int UserDbId = -1;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _audioListener = GetComponentInChildren<AudioListener>();
        _playersManagement = FindObjectOfType<PlayersManagement>();

        if (!IsOwner) //Se chi richiama questo metodo non è il proprietario allora...
        {
            _audioListener.enabled = false; //...viene disabilitato l'audiolistener
            _camera.enabled = false; //...viene disabilitata la camera
            _playersManagement.enabled = false; //...viene disabilitato questo script

            //vengono disabilitati questi componeti
            GetComponent<NetworkObject>().enabled = false;
            GetComponent<NetworkPlayer>().enabled = false;
            GetComponent<DamageManager>().enabled = false;
            GetComponent<PlayerInteract>().enabled = false;
            GetComponent<PlayerMotor>().enabled = false;
            GetComponent<PlayerLook>().enabled = false;
            GetComponent<PlayerInteract>().enabled = false;
            return;
        }
        NetworkId = GetComponent<NetworkObject>().NetworkObjectId; //vado a prendere dal server il mio id
        UserDbId = PlayerPrefs.GetInt("user_id");
        _playersManagement.ClientConnectedServerRpc(NetworkId); //avviso il server che mi sono connesso
    }

    private void Update()
    {
        if (!IsOwner) return; //se non sono l'owner non faccio nulla
        if (IsHost && NetworkManager.Singleton.ShutdownInProgress) //Se l'host crasha 
        {
            Debug.LogError("Singleton shutdown in corso (server)");
            DisconnectAllClientsServerRpc();
        }
        if (Input.GetKey(KeyCode.T)) //match end example
        {
            if (IsHost) //match terminato (no errore)
            {
                DisconnectAllClientsServerRpc();
            }
        }
        if (Input.GetKey(KeyCode.Escape)) //crash example
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > 2f)
            {
                deltaTime = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                NetworkManager.Singleton.Shutdown();
            }
        }
    }

    [ServerRpc(RequireOwnership = true)]
    private void DisconnectAllClientsServerRpc() //disconnetto tutti i giocatori
    {
        EndAllConnectionsClientRpc();
    }

    [ClientRpc]
    private void EndAllConnectionsClientRpc()
    {
        if (!IsOwner)
            return;
        NetworkManager.Singleton.Shutdown();
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient && IsLocalPlayer)
        {
            ulong netId = GetComponent<NetworkObject>().NetworkObjectId;
            int playerScore = 0; //query
            PlayerPrefs.SetInt("score", playerScore);
            SceneManager.LoadScene(2);
            NetworkManager.Singleton.Shutdown();
        }
    }

    
}

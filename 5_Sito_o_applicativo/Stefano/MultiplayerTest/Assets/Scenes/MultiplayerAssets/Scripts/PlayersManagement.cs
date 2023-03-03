using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Newtonsoft.Json;
using System.Linq;

public class PlayersManagement : NetworkBehaviour
{
    private IDictionary<ulong, GameObject> players;
    NetworkVariable<int> clientsConnected;

    private void Awake()
    {
        clientsConnected = new NetworkVariable<int>(0);
    }
    private void Start()
    {
        players = new Dictionary<ulong, GameObject>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientConnectedServerRpc(ulong clientId)
    {
        GameObject[] rawList = GameObject.FindGameObjectsWithTag("Player");
        GameObject clientObj = null;
        foreach (GameObject obj in rawList)
        {
            if(obj.GetComponent<NetworkObject>().NetworkObjectId == clientId)
            {
                clientObj = obj;
                break;
            }
        }
        try
        {
            players.Add(clientId, clientObj);
            UpdateDictCount();
            Debug.Log("Client connected, id: " + clientId);
        }
        catch { }
        

    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientDisconnectedServerRpc(ulong clientId)
    {
        Debug.Log("Client disconnected, id: " + clientId);
        players.Remove(clientId);
        UpdateDictCount();
    }

    public int GetNumberOfClients()
    {
        return clientsConnected.Value;
    }

    public IDictionary<ulong, GameObject> GetPlayers()
    {
        return players;
    }

    private void UpdateDictCount()
    {
        clientsConnected.Value = players.Count;
    }
}

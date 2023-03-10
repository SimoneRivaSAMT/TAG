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
    NetworkVariable<ulong> player1Id;
    NetworkVariable<ulong> player2Id;
    NetworkVariable<ulong> player3Id;
    NetworkVariable<ulong> player4Id;

    private void Awake()
    {
        clientsConnected = new NetworkVariable<int>(0);
        player1Id = new NetworkVariable<ulong>(999);
        player2Id = new NetworkVariable<ulong>(999);
        player3Id = new NetworkVariable<ulong>(999);
        player4Id = new NetworkVariable<ulong>(999);
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
            ulong cId = clientId;
            if (clientId == 1)
                cId = 0;
            else
                cId -= (cId - 1);
            switch (cId)
            {
                case 0:
                    player1Id.Value = cId;
                    break;

                case 1:
                    player2Id.Value = cId;
                    break;

                case 2:
                    player3Id.Value = cId;
                    break;

                case 3:
                    player4Id.Value = cId;
                    break;

                default:
                    Debug.LogError("Error, clientId > 4! Actual ID: " + cId);
                    break;
            }
            Debug.Log("Joined client " + cId);
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

    public ulong GetClientId(int index)
    {
        switch (index)
        {
            case 0:
                return player1Id.Value;
            case 1:
                return player2Id.Value;
            case 2:
                return player3Id.Value;
            case 3:
                return player4Id.Value;
        }
        return 999;
    }
    private void UpdateDictCount()
    {
        clientsConnected.Value = players.Count;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DamageManager : NetworkBehaviour
{
    private PlayersManagement playersManagement;
    private void Start()
    {
        playersManagement = FindObjectOfType<PlayersManagement>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHittedServerRpc(ulong playerNetId)
    {
        PlayerHittedClientRpc(playerNetId);
    }

    [ClientRpc]
    public void PlayerHittedClientRpc(ulong playerNetId)
    {
        ulong clientId = 0;
        Debug.LogError("My net id: " + clientId + " / playerNetId recived: " + playerNetId);
        if(playerNetId == clientId)
        {
            Debug.LogError("Colpito");
            //MapGenerator mg = FindObjectOfType<MapGenerator>();
            //mg.GenerateMapServerRpc();
            //GetComponent<NetworkPlayer>().TakeDamage();
        }
    }
}

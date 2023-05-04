using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class MatchManager : NetworkBehaviour
{
    private PlayerManager playerManager;
    private List<GameObject> players; 
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner || !IsHost)
            return;
        playerManager = new PlayerManager();
        players = playerManager.GetPlayers();
        int i = 0;
        foreach(GameObject player in players)
        {
            if(player.tag == "Player")
                player.transform.position = playerManager.GetRespawnPositions()[i].transform.position;
            else
                player.transform.position = player.GetComponent<NavMeshAgent>().nextPosition = playerManager.GetRespawnPositions()[i].transform.position;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

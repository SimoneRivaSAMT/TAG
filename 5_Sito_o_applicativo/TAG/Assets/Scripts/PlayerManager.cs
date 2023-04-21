using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    /*private GameObject player;
    public PlayerManager(GameObject player)
    {
        this.player = player;
    }*/
    public List<GameObject> GetPlayers()
    {
        // Create list of all Player and AI Enemies
        List<GameObject> players = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            players.Add(enemy);
        }
        return players;
    }

    public void RespawnPlayer(GameObject player)
    {
        player.GetComponent<MeshRenderer>().enabled = false;
        if(player.GetComponent<CharacterController>() != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
        }
    }
}

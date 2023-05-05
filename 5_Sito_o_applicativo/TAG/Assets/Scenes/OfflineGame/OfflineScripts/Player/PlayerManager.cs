using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager
{
    // Create list of all Player and AI Enemies
    public List<GameObject> GetPlayers()
    {
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

    public void DespawnPlayer(GameObject player)
    {
        if (player.GetComponent<CharacterController>() != null)
        {

            ChangePlayerState(player, false);
        }
        else
        {
            ChangeEnemyState(player, false);
        }
    }

    public void RespawnPlayer(GameObject player)
    {
        if (player.GetComponent<CharacterController>() != null)
        {

            ChangePlayerState(player, true);
        }
        else
        {
            ChangeEnemyState(player, true);
        }
    }

    // Enable / Disable functionality when eliminated (Enemy)
    public void ChangeEnemyState(GameObject player, bool isEnabled)
    {
        player.GetComponent<MeshRenderer>().enabled = isEnabled;
        player.transform.Find("Laser").GetComponent<MeshRenderer>().enabled = isEnabled;
        player.GetComponentInChildren<EnemyLaserSystem>().enabled = isEnabled;
        if (isEnabled)
        {
            player.tag = "Enemy";
            player.transform.position = GetRandomRespawnPosition();
        }
        else
        {
            player.tag = "Eliminated";
        }
    }

    // Enable / Disable functionality when eliminated (Player)
    public void ChangePlayerState(GameObject player, bool isEnabled)
    {
        player.GetComponent<MeshRenderer>().enabled = isEnabled;
        player.transform.Find("CameraHolder").Find("MainCamera").Find("Laser").GetComponent<MeshRenderer>().enabled = isEnabled;
        player.transform.Find("CameraHolder").Find("MainCamera").Find("Shield").GetComponent<MeshRenderer>().enabled = isEnabled;
        player.GetComponent<InputManager>().enabled = isEnabled;
        if (isEnabled)
        {
            player.tag = "Player";
            player.transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text = "";
            player.transform.position = GetRandomRespawnPosition();
        }
        else
        {
            player.transform.Find("PlayerUI").Find("PromptText").GetComponent<TextMeshProUGUI>().text = "Eliminated";
            player.tag = "Eliminated";
        }
    }

    public GameObject[] GetRespawnPositions()
    {
        return GameObject.FindGameObjectsWithTag("RespawnPoint");
    }

    public Vector3 GetRandomRespawnPosition()
    {
        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("RespawnPoint");
        int rnd = Random.Range(0, respawnPoints.Length);
        return respawnPoints[rnd].transform.position;
    }
}

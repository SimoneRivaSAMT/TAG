using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Assets.CasualMap;
using System.Linq;

public class DamageManager : NetworkBehaviour
{
    private const int ON_DEAD_SCORE = 8;
    private const int ON_KILL_SCORE = 6;
    private const int ON_SPAWN_DEFAULT_LIFE = 10;
    private const int ON_SPAWN_SCORE = 100;

    NetworkVariable<int> p1Life;
    NetworkVariable<int> p2Life;
    NetworkVariable<int> p3Life;
    NetworkVariable<int> p4Life;

    NetworkVariable<int> p1Score;
    NetworkVariable<int> p2Score;
    NetworkVariable<int> p3Score;
    NetworkVariable<int> p4Score;

    GameObject lives;

    TMP_Text[] playerLives;

    private void Awake()
    {
        p1Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p2Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p3Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p4Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);

        p1Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p2Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p3Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p4Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        
        lives = GameObject.FindGameObjectWithTag("player_lives_ui");
        playerLives = lives.GetComponentsInChildren<TMP_Text>();
        playerLives = playerLives.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        if (IsHost)
            UpdateTextClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHittedServerRpc(ulong playerNetId)
    {
        if (!IsOwner)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject gameObject in gos)
            {
                if(gameObject.GetComponent<NetworkObject>().NetworkObjectId == 1)
                {
                    gameObject.GetComponent<DamageManager>().PlayerHittedServerRpc(playerNetId);
                    return;
                }
            }
        }
        Debug.LogError("Il player " + playerNetId + " è stato colpito! // ServerID: " + GetComponent<NetworkObject>().NetworkObjectId);
        switch (playerNetId)
        {
            case 1:
                p1Life.Value--;
                if(p1Life.Value == 0)
                {
                    p1Score.Value -= ON_DEAD_SCORE;
                    p1Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 6:
                p2Life.Value--;
                if (p2Life.Value == 0)
                {
                    p2Score.Value -= ON_DEAD_SCORE;
                    p2Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 7:
                p3Life.Value--;
                if (p3Life.Value == 0)
                {
                    p3Score.Value -= ON_DEAD_SCORE;
                    p3Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 8:
                p4Life.Value--;
                if (p4Life.Value == 0)
                {
                    p4Score.Value -= ON_DEAD_SCORE;
                    p4Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
        }
    }

    [ClientRpc]
    private void UpdateTextClientRpc()
    {
        playerLives[0].text = p1Life.Value + "";
        playerLives[1].text = p2Life.Value + "";
        playerLives[2].text = p3Life.Value + "";
        playerLives[3].text = p4Life.Value + "";
    }
}

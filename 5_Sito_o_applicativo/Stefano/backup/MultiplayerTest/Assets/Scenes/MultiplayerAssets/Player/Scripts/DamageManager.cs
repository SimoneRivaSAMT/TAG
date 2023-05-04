using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Assets.CasualMap;
using System.Linq;
using UnityEngine.Networking;
using System;

/*
 Questo script gestisce il danno da raycast.
 Potrebbe essere necessario implementare lo score se non funzionante
 */

public class DamageManager : NetworkBehaviour
{
    private const int ON_DEAD_SCORE = 10;
    private const int ON_KILL_SCORE = 25;
    private const int ON_SPAWN_DEFAULT_LIFE = 100;
    private const int ON_SPAWN_SCORE = 0;
    private const int ON_TAG_DAMAGE = 5;

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
        //Istanzio le variabili con valore di default
        p1Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p2Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p3Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);
        p4Life = new NetworkVariable<int>(ON_SPAWN_DEFAULT_LIFE);

        p1Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p2Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p3Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        p4Score = new NetworkVariable<int>(ON_SPAWN_SCORE);
        
        //cerco le scritte della vita rimasta <UI>
        lives = GameObject.FindGameObjectWithTag("player_lives_ui");
        playerLives = lives.GetComponentsInChildren<TMP_Text>();
        playerLives = playerLives.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        if (IsHost)
        {
            UpdateTextClientRpc(); //l'host ordina di fare l'update del testo ai client
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log(string.Format("Player1Score: {0} / Player2Score: {1}", 
                    p1Score.Value, p2Score.Value));
            }
        } 
    }

    public int[] GetPlayerScores()
    {
        return new int[] { p1Score.Value, p2Score.Value, p3Score.Value, p4Score.Value };
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHittedServerRpc(ulong playerNetId, ulong hittedByPlayerId) //metodo che avvisa il server che il client è stato colpito
    {
        if (!FindObjectOfType<NetMatchManager>().isMatchStarted.Value)
            return;
        //Mi assicuro che sia il giocatore "fisico" ad eseguire questo <bug di netcode>
        if (!IsOwner)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject gameObject in gos)
            {
                if(gameObject.GetComponent<NetworkObject>().NetworkObjectId == 1)
                {
                    gameObject.GetComponent<DamageManager>().PlayerHittedServerRpc(playerNetId, hittedByPlayerId);
                    return;
                }
            }
        }
        Debug.LogError("Il player " + playerNetId + " è stato colpito da " + hittedByPlayerId + "! // ServerID: " + GetComponent<NetworkObject>().NetworkObjectId);
        //setto le variabili in base a chi è stato colpito
        bool playerDied = false;
        switch (playerNetId)
        {
            case 1:
                p1Life.Value -= ON_TAG_DAMAGE;
                if(p1Life.Value == 0)
                {
                    playerDied = true;

                    if (p1Score.Value - ON_DEAD_SCORE <= 0)
                        p1Score.Value = 0;
                    else
                        p1Score.Value -= ON_DEAD_SCORE;

                    p1Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 6:
                p2Life.Value -= ON_TAG_DAMAGE;
                if (p2Life.Value == 0)
                {
                    playerDied = true;

                    if (p2Score.Value - ON_DEAD_SCORE <= 0)
                        p2Score.Value = 0;
                    else
                        p2Score.Value -= ON_DEAD_SCORE;
                    p2Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 7:
                p3Life.Value -= ON_TAG_DAMAGE;
                if (p3Life.Value == 0)
                {
                    playerDied = true;

                    if (p3Score.Value - ON_DEAD_SCORE <= 0)
                        p3Score.Value = 0;
                    else
                        p3Score.Value -= ON_DEAD_SCORE;

                    p3Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
            case 8:
                p4Life.Value -= ON_TAG_DAMAGE;
                if (p4Life.Value == 0)
                {
                    playerDied = true;

                    if (p4Score.Value - ON_DEAD_SCORE <= 0)
                        p4Score.Value = 0;
                    else
                        p4Score.Value -= ON_DEAD_SCORE;

                    p4Life.Value = ON_SPAWN_DEFAULT_LIFE;
                }
                break;
        }
        if (playerDied)
        {
            PlayerTaggedBySomeoneClientRpc(hittedByPlayerId);
            AddDeathPointsServerRpc(hittedByPlayerId);
        }
    }

    [ServerRpc(RequireOwnership = true)]
    private void AddDeathPointsServerRpc(ulong netId) //quando elimini qualcuno ti da punti
    {
        switch (netId)
        {
            case 1:
                p1Score.Value += ON_KILL_SCORE;
                break;
            case 6:
                p2Score.Value += ON_KILL_SCORE;
                break;
            case 7:
                p3Score.Value += ON_KILL_SCORE;
                break;
            case 8:
                p4Score.Value += ON_KILL_SCORE;
                break;
        }
    }

    [ClientRpc]
    private void UpdateTextClientRpc() //ogni client aggiorna i testi
    {
        playerLives[0].text = p1Life.Value + "";
        playerLives[1].text = p2Life.Value + "";
        playerLives[2].text = p3Life.Value + "";
        playerLives[3].text = p4Life.Value + "";
    }
    [ClientRpc]
    private void PlayerTaggedBySomeoneClientRpc(ulong playerThatTaggedId)
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (item.GetComponent<NetworkPlayer>().IsOwner)
            {
                if (playerThatTaggedId == item.GetComponent<NetworkPlayer>().NetworkObjectId)
                    Debug.LogError("ho ucciso io");
            }
        }
    }
}

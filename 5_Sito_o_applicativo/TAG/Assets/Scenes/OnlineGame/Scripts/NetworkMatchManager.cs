using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using Assets.Scenes;

public class NetworkMatchManager : NetworkBehaviour
{

    public NetworkVariable<bool> isMatchStarted;
    public int matchId;

    private int[] scores;
    private bool isTopScorer = false;

    private void Awake()
    {
        scores = new int[4];
        isMatchStarted = new NetworkVariable<bool>(false);
        matchId = -1;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    EndMatchServerRpc();
        //}
    }

    public void StartMatch()
    {
        if (!IsHost)
            return;
        StartMatchServerRpc();
    }

    public void EndMatch()
    {
        if (!IsHost)
            return;
        EndMatchServerRpc();
    }

    [ServerRpc]
    private void StartMatchServerRpc()
    {
        if (isMatchStarted.Value || !IsHost)
            return;

        isMatchStarted.Value = true;
        StartCoroutine(SaveMatchOnDb());
    }

    [ServerRpc]
    private void EndMatchServerRpc()
    {
        DamageManager dm = null;
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (item.GetComponent<NetworkPlayer>().IsOwner)
            {
                dm = item.GetComponent<DamageManager>();
                break;
            }
        }
        scores = dm.GetPlayerScores();
        SetScoresClientRpc(scores);
        isMatchStarted.Value = false;
        InsertScoresOnDbClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        Debug.Log("inserisco plays");
        StartCoroutine(AddPlaysOnDb());
    }

    [ClientRpc]
    private void InsertScoresOnDbClientRpc()
    {
        int dbid = GetUserId();
        ulong nid = GetNetworkObjId();
        Debug.LogError("inserisco scores, sono DbId: " + dbid);
        switch (nid)
        {
            case 1:
                IsTopScorer(nid);
                StartCoroutine(InsertScoreOnDb(scores[0]));
                break;
            case 6:
                IsTopScorer(nid);
                StartCoroutine(InsertScoreOnDb(scores[1]));
                break;
            case 7:
                IsTopScorer(nid);
                StartCoroutine(InsertScoreOnDb(scores[2]));
                break;
            case 8:
                IsTopScorer(nid);
                StartCoroutine(InsertScoreOnDb(scores[3]));
                break;
        }
        
    }

    private void IsTopScorer(ulong netId)
    {
        switch (netId)
        {
            case 1:
                if(scores[0] > scores[1] && scores[0] > scores[2] && scores[0] > scores[3])
                {
                    isTopScorer = true;
                }
                break;
            case 6:
                if (scores[1] > scores[0] && scores[1] > scores[2] && scores[1] > scores[3])
                {
                    isTopScorer = true;
                }
                break;
            case 7:
                if (scores[2] > scores[0] && scores[2] > scores[1] && scores[2] > scores[3])
                {
                    isTopScorer = true;
                }
                break;
            case 8:
                if (scores[3] > scores[2] && scores[3] > scores[1] && scores[1] > scores[0])
                {
                    isTopScorer = true;
                }
                break;
        }
    }

    [ClientRpc]
    private void SetMatchIdClientRpc(int id)
    {
        matchId = id;
        PlayerPrefs.SetInt("last_match_id", id);
    }

    [ClientRpc]
    private void SetScoresClientRpc(int[] _scores)
    {
        scores = _scores;
    }

    private int GetUserId()
    {
        //ciclo tutti i player perché netcode ogni tanto torna quello sbagliato <bug di netcode>
        //io cerco solo quello "fisico"
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkPlayer>().UserDbId != -1)
            {
                return player.GetComponent<NetworkPlayer>().UserDbId;
            }

        }
        return 0;
    }

    private ulong GetNetworkObjId()
    {
        //ciclo tutti i player perché netcode ogni tanto torna quello sbagliato <bug di netcode>
        //io cerco solo quello "fisico"
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (item.GetComponent<NetworkPlayer>().IsOwner)
                return item.GetComponent<NetworkPlayer>().NetworkObjectId;
        }
        return 0;
    }

    private IEnumerator AddPlaysOnDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest req;
        int user_id = GetUserId();
        Debug.Log("Match_ID: " + matchId);
        int matchid = matchId;
        form.AddField("uid", user_id);
        form.AddField("mid", matchid);
        req = UnityWebRequest.Post("http://localhost/scoreManager/add/", form);
        yield return req.SendWebRequest();
        Debug.Log(req.result);
        Debug.Log(req.downloadHandler.text);
        if(req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error + " // URL: " + req.url);
        }
    }

    private IEnumerator SaveMatchOnDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest req;
        form.AddField("date_played", GetTimestamp(DateTime.Now));
        req = UnityWebRequest.Post("http://localhost/matchManager/startMatch/", form);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            int matchid = Convert.ToInt32(req.downloadHandler.text);
            Debug.Log("Match avviato!");
            Debug.Log("match_id = " + matchid);
            isMatchStarted.Value = true;
            matchId = matchid;
            SetMatchIdClientRpc(matchid);
            StartGameClientRpc();
        }
        else
        {
            Debug.LogError("Error: " + req.error + " // URL: " + req.url);
        }
    }

    private IEnumerator InsertScoreOnDb(int _score)
    {
        int user_id = GetUserId();
        int match_id = matchId;
        int score = _score;
        WWWForm form = new WWWForm();
        UnityWebRequest req;
        form.AddField("user_id", user_id);
        form.AddField("match_id", match_id);
        form.AddField("score", score);
        PlayerPrefs.SetInt("last_score", score);
        req = UnityWebRequest.Post("http://localhost/scoreManager/update", form);
        yield return req.SendWebRequest();
        Debug.LogError(req.downloadHandler.text);
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + req.error + " // URL: " + req.url);
        }
        if (isTopScorer)
        {
            PlayerPrefs.SetInt("is_top_scorer", 1);
        }
        else
        {
            PlayerPrefs.SetInt("is_top_scorer", 0);
        }
        NetworkManager.Singleton.Shutdown();
    }

    private static string GetTimestamp(DateTime value)
    {
        return value.ToString("yyyyMMddHHmmss");
    }
}

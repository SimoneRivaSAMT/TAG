using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System;

public class LeaderboardManager : MonoBehaviour
{
    private string json;
    GameObject modifiedObject;
    [Header("Obj")]
    public GameObject prefab;
    public GameObject container;

    private void Start()
    {
        StartCoroutine(GetAllMatchesId());
    }

    private void DisplayMatch()
    {
        Instantiate(modifiedObject, container.transform);
    }

    private IEnumerator GetAllMatchesId()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest req;
        req = UnityWebRequest.Post(GlobalVars.BASE_URL + "scoreManager/getMatchIds/", form);
        yield return req.SendWebRequest();
        if(req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(req.downloadHandler.text);
            foreach (string id in req.downloadHandler.text.Split(';'))
            {
                if (id.Length > 0)
                    StartCoroutine(GetMatchesFromDb(Convert.ToInt32(id)));
            }
        }
        else
        {
            Debug.LogError("Error: " + req.error + " // URL: " + req.url);
        }
        
    }

    private IEnumerator GetMatchesFromDb(int id)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest req;
        form.AddField("match_id", id);
        req = UnityWebRequest.Post(GlobalVars.BASE_URL + "scoreManager/getLeaderboard/", form);
        yield return req.SendWebRequest();
        
        if(req.result == UnityWebRequest.Result.Success)
        {
            json = req.downloadHandler.text;
            Debug.Log(json);
            SingleMatch match = JsonUtility.FromJson<SingleMatch>(json);
            modifiedObject = prefab;
            foreach (TMP_Text item in modifiedObject.GetComponentsInChildren<TMP_Text>())
            {
                if (item.text == "p1n")
                    item.text = match.P1Nick.Length == 0 ? "----" : match.P1Nick;
                if (item.text == "p2n")
                    item.text = match.P2Nick.Length == 0 ? "----" : match.P2Nick;
                if (item.text == "p3n")
                    item.text = match.P3Nick.Length == 0 ? "----" : match.P3Nick;
                if (item.text == "p4n")
                    item.text = match.P4Nick.Length == 0 ? "----" : match.P4Nick;
                if (item.text == "<date>")
                    item.text = "Played the " + match.DatePlayed;


                if (item.text == "p1s")
                    item.text = match.P1Score.ToString();
                if (item.text == "p2s")
                    item.text = match.P2Score.ToString();
                if (item.text == "p3s")
                    item.text = match.P3Score.ToString();
                if (item.text == "p4s")
                    item.text = match.P4Score.ToString();

            }
            DisplayMatch();
        }
        else
        {
            Debug.LogError("Error: " + req.error + " // URL: " + req.url);
        }
    }
}

internal class SingleMatch
{
    public string DatePlayed;
    public int P1Score;
    public int P2Score;
    public int P3Score;
    public int P4Score;
    public string P1Nick;
    public string P2Nick;
    public string P3Nick;
    public string P4Nick;
}

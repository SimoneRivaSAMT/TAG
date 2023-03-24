using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowFreeLobbies : MonoBehaviour
{
    public GameObject layoutPrefab;
    public GameObject table;

    private string BASE_URL;
    private string jsonlobbies;

    private void Awake()
    {
        BASE_URL = FindObjectOfType<StartGame>().BASE_URL;
    }
    public void DisplayLobbies()
    {
        StartCoroutine(GetLobbiesFromDb());;
    }

    private void CreateTable()
    {
        List<FreeMatch> freeMatches = new List<FreeMatch>();
        Debug.Log(jsonlobbies);
        foreach (string json in jsonlobbies.Split(';'))
        {
            FreeMatch match = JsonUtility.FromJson<FreeMatch>(json);
            freeMatches.Add(match);
            Instantiate(layoutPrefab, table.transform);
        }
        FindObjectOfType<AutoStacker>().UpdateAll();
    }

    private IEnumerator GetLobbiesFromDb()
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "get-lobbies");
        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "app/vacant_match.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            jsonlobbies = www.downloadHandler.text;
            CreateTable();
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString());
            jsonlobbies = "error";
        }
    }

}

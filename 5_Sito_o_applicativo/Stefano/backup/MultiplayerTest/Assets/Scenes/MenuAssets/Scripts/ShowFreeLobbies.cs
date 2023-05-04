using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class ShowFreeLobbies : MonoBehaviour
{
    public GameObject layoutPrefab;
    public GameObject table;
    public Button reloadButton;

    private string BASE_URL;
    private string jsonlobbies;
    private Transform tableTransform;

    private void Awake()
    {
        BASE_URL = FindObjectOfType<StartGame>().BASE_URL;
    }

    private void Start()
    {
        tableTransform = table.transform;
    }

    public void DisplayLobbies()
    {
        reloadButton.GetComponentInChildren<TMP_Text>().text = "Loading data...";
        reloadButton.interactable = false;
        StartCoroutine(GetLobbiesFromDb());;
    }

    private void CreateTable()
    {
        List<FreeMatch> freeMatches = new List<FreeMatch>();
        Debug.Log(jsonlobbies);
        if (jsonlobbies.Length > 0)
        {
            foreach (string json in jsonlobbies.Split(';'))
            {
                FreeMatch match = JsonUtility.FromJson<FreeMatch>(json);
                freeMatches.Add(match);
                GameObject inst = Instantiate(layoutPrefab, tableTransform);
                inst.GetComponentInChildren<TMP_Text>().text = match.Nickname;
                inst.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    FindObjectOfType<StartGame>().StartAsClient(match.IpAddress);
                });
            }
            FindObjectOfType<AutoStacker>().UpdateAll();
        }
    }

    private IEnumerator GetLobbiesFromDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "matchManager/manageVacant/getLobbies", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            jsonlobbies = www.downloadHandler.text;
            StartCoroutine(DeleteView());
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString());
            Debug.LogError("link: " + www.url);
            jsonlobbies = "error";
        }
    }

    private IEnumerator DeleteView()
    {
        FindObjectOfType<AutoStacker>().RemoveAllRows();
        yield return new WaitForSeconds(.5f);
        CreateTable();
        reloadButton.GetComponentInChildren<TMP_Text>().text = "Reload data";
        reloadButton.interactable = true;
    }
}

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

    private string jsonlobbies;
    private Transform tableTransform;

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
        }
    }

    private IEnumerator GetLobbiesFromDb()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(GlobalVars.BASE_URL + "matchManager/manageVacant/getLobbies", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            jsonlobbies = www.downloadHandler.text;
            StartCoroutine(ReloadView());
        }
        else
        {
            jsonlobbies = "error";
        }
    }

    private IEnumerator ReloadView()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("JoinPrefab"))
        {
            Destroy(item);
        }
        yield return new WaitForSeconds(.5f);
        CreateTable();
        reloadButton.GetComponentInChildren<TMP_Text>().text = "Load lobbies";
        reloadButton.interactable = true;
    }
}

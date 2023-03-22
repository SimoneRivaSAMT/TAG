using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/*
 Script che gestisce il menu. PER OGNI PLAYERPREFS USARE UN ENUM
 */
public class StartGame : MonoBehaviour
{
    public TMP_InputField ipAddressInput;
    public TMP_InputField usernameInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void GetLobbies()
    {
        StartCoroutine(GetLobbiesFromDb());
    }

    public void StartAsHost()
    {
        StartCoroutine(AddLobbyOnDb());
    }

    public void StartAsClient()
    {
        if(IsValidIP(ipAddressInput.text))
        {
            PlayerPrefs.SetString("multiplayerMode", "client");
            PlayerPrefs.SetString("connectIp", ipAddressInput.text);
            SceneManager.LoadScene(1);
        }
        else
        {
            ipAddressInput.image.color = Color.red;
        }
    }

    private bool IsValidIP(string Address)
    {
        //Match pattern for IP address    
        string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        //Regular Expression object    
        Regex check = new Regex(Pattern);

        //check to make sure an ip address was provided    
        if (string.IsNullOrEmpty(Address))
            //returns false if IP is not provided    
            return false;
        else
            //Matching the pattern    
            return check.IsMatch(Address, 0);
    }

    private IEnumerator AddLobbyOnDb()
    {
        WWWForm form = new WWWForm();
        string myIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
        form.AddField("username", usernameInput.text.Length > 0 ? usernameInput.text.ToString() : "no_name");
        form.AddField("ip_address", myIp);
        form.AddField("action", "add-lobby");
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/tag_www/app/vacant_match.php", form);
        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetString("multiplayerMode", "host");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString());
        }
    }

    private IEnumerator GetLobbiesFromDb()
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "get-lobbies");
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/tag_www/app/vacant_match.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString());
        }
    }
}

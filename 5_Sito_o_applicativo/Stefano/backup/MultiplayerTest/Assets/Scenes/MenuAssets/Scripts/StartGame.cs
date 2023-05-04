using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

/*
 Script che gestisce il menu. PER OGNI PLAYERPREFS USARE UN ENUM
 */
public class StartGame : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public string BASE_URL;
    private string jsonlobbies;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void StartAsHost()
    {
        StartCoroutine(AddLobbyOnDb());
    }

    public void StartAsClient(string ipAddress = "")
    {

        PlayerPrefs.SetString("multiplayerMode", "client");
        PlayerPrefs.SetString("connectIp", ipAddress);
        SceneManager.LoadScene(1);
    }
    

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }



    private IEnumerator AddLobbyOnDb()
    {
        //Non funziona con il Proxy, da sistemare

        //UnityWebRequest getIp = UnityWebRequest.Post("https://api.ipify.org", new WWWForm());
        //yield return getIp.SendWebRequest();
        //if(getIp.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.LogError("getIp error! " + getIp.error.ToString() + " url: " + getIp.url);
        //    StopCoroutine(AddLobbyOnDb());
        //}
        //string myIp = getIp.downloadHandler.text;

        string myIp = GetLocalIPAddress();
        WWWForm form = new WWWForm();

        form.AddField("username", usernameInput.text.Length > 0 ? usernameInput.text.ToString() : "no_name");
        form.AddField("ip_address", myIp);
        form.AddField("user_id", PlayerPrefs.GetInt("user_id"));
        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "matchManager/manageVacant/addLobby", form);
        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetString("multiplayerMode", "host");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString() + " url: " + www.url);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using Assets.PlayerPreferences;
using Assets.Scenes;
using System.Net.NetworkInformation;

/*
 Script che gestisce il menu. PER OGNI PLAYERPREFS USARE UN ENUM
 */
public class StartGame : MonoBehaviour
{
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
        PlayerPrefs.DeleteKey("connectIp");
        PlayerPrefs.SetString("multiplayerMode", "client");
        PlayerPrefs.SetString("connectIp", ipAddress);
        Debug.Log(ipAddress);
        SceneManager.LoadScene((int)SceneToId.onlineGame);
    }


    public static string GetLocalIPAddress()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback && ni.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProps = ni.GetIPProperties();
                if (ipProps.GatewayAddresses.Count > 0)
                {
                    foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return "Indirizzo IP locale non trovato!";
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
        string ip = GetLocalIPAddress();
        WWWForm form = new WWWForm();
        PlayerPrefs.DeleteKey("connectIp");
        PlayerPrefs.SetString("connectIp", ip);
        form.AddField("username", PlayerPrefs.GetString(PlayerPreference.USER_UNAME));
        form.AddField("ip_address", ip);
        form.AddField("user_id", PlayerPrefs.GetInt(PlayerPreference.USER_ID));
        UnityWebRequest www = UnityWebRequest.Post(GlobalVars.BASE_URL + "matchManager/manageVacant/addLobby", form);
        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetString("multiplayerMode", "host");
            SceneManager.LoadScene((int)SceneToId.onlineGame);
        }
        else
        {
            Debug.LogError("www error! " + www.error.ToString() + " url: " + www.url);
        }
    }
}

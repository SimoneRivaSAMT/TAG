using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;

public class NetManager : MonoBehaviour
{
    public TextMeshProUGUI textIP;
    private void Start()
    {
        textIP.text = "IP: " + GetLocalIPAddress();
        string mode = PlayerPrefs.GetString("multiplayerMode");
        print(mode);
        switch (mode)
        {
            case "server":
                Debug.LogError("Impossible to instance a server! Not Supported!");
                Application.Quit();
                break;
            case "host":
                textIP.text += " (HOST)";
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    "0.0.0.0",
                    (ushort)6969,
                    PlayerPrefs.GetString("connectIp")
                 );
                NetworkManager.Singleton.StartHost();
                break;
            case "client":
                textIP.text += " (CLIENT)";
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    PlayerPrefs.GetString("connectIp"),
                    (ushort)6969
                 );
                NetworkManager.Singleton.StartClient();
                break;
        }
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
}

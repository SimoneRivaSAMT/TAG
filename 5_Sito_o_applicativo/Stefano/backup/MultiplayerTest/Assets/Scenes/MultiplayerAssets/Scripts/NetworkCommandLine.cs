using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;
using TMPro;

/*
 Non in uso
 */

public class NetworkCommandLine : MonoBehaviour
{

    public TextMeshProUGUI textIP;

    private NetworkManager netManager;
    private UnityTransport unityTransport;

    void Start()
    {
        //netManager = GetComponentInParent<NetworkManager>();
        //unityTransport = GetComponentInParent<UnityTransport>();
        //textIP.text = "IP: " + GetLocalIPAddress();
        //if (Application.isEditor) return;
        
        ////var args = GetCommandlineArgs();

        //string mode = PlayerPrefs.GetString("multiplayerMode");
        //print(mode);
        //switch (mode)
        //{
        //    case "server":
        //        NetworkManager.Singleton.StartServer();
        //        textIP.text += " (SRV)";
        //        break;
        //    case "host":
        //        textIP.text += " (HOST)";
        //        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        //            PlayerPrefs.GetString("connectIp"),
        //            (ushort)6969
        //         );
        //        NetworkManager.Singleton.StartHost();
        //        break;
        //    case "client":
        //        textIP.text += " (CLI)";
        //        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        //            PlayerPrefs.GetString("connectIp"),
        //            (ushort)6969
        //         );
        //        NetworkManager.Singleton.StartClient();
        //        break;
        //}
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;

/*
 Script che gestisce il join e la creazione di host
 */
public class NetManager : MonoBehaviour
{
    public TextMeshProUGUI textIP;
    private void Start()
    {
        textIP.text = "IP: " + GetLocalIPAddress();
        string mode = PlayerPrefs.GetString("multiplayerMode"); //USARE ENUM
        print(mode);
        switch (mode)
        {
            case "server":
                Debug.LogError("Impossible to instance a server! Not Supported!");
                Application.Quit();
                break;
            case "host":
                textIP.text += " (HOST)";
                //Setto le impostazioni di rete
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    "0.0.0.0", //dove connettersi. è host, quindi non server. lasciare 0.0.0.0
                    (ushort)6969, //porta di ascolto
                    PlayerPrefs.GetString("connectIp") //indirizzo ip del host. USARE ENUM
                 );
                //Avvio il processo di rete e mi connetto
                NetworkManager.Singleton.StartHost();
                break;
            case "client":
                textIP.text += " (CLIENT)";
                //Setto le impostazioni di rete
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    PlayerPrefs.GetString("connectIp"), //indirizzo ip del host. USARE ENUM
                    (ushort)6969 //porta del host
                 );
                //Avvio il processo di rete e mi connetto
                NetworkManager.Singleton.StartClient();
                break;
        }
    }

    private static string GetLocalIPAddress() //serve a ottenere l'indirizzo privato. 
                                              //in futuro campbiarlo e ottenere quello pubblico
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

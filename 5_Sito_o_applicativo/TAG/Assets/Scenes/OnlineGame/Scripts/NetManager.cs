using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;
using System.Net.NetworkInformation;

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
                    (ushort)7000, //porta di ascolto
                    PlayerPrefs.GetString("connectIp") //indirizzo ip del host. USARE ENUM
                 );
                Debug.LogError("Host attivo sull'indirizzo " + PlayerPrefs.GetString("connectIp"));
                //Avvio il processo di rete e mi connetto
                NetworkManager.Singleton.StartHost();
                break;
            case "client":
                textIP.text += " (CLIENT)";
                //Setto le impostazioni di rete
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    PlayerPrefs.GetString("connectIp"), //indirizzo ip del host.-
                    (ushort)7000 //porta del host
                 );
                Debug.LogError("Client attio sull'indirizzo " + PlayerPrefs.GetString("connectIp"));
                //Avvio il processo di rete e mi connetto
                NetworkManager.Singleton.StartClient();
                break;
        }
    }

    //serve a ottenere l'indirizzo privato. 
    //in futuro campbiarlo e ottenere quello pubblic                                
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

}

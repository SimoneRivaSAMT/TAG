using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

/*
 Script che gestisce il lifetime del server
 */
public class ServerLife : NetworkBehaviour
{
    private NetworkVariable<float> timePast;
    public TextMeshPro text;

    private void Awake()
    {
        timePast = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone);
    }

    private void Update()
    {
        if (FindObjectOfType<NetMatchManager>().isMatchStarted.Value && IsHost)
            timePast.Value += Time.deltaTime;
        text.text = "Server Life: " + (int)timePast.Value + " seconds";
    }
}

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
    private NetworkVariable<float> timeRemaining;
    public TextMeshPro text;
    public float matchDurationSeconds = 20;

    private void Awake()
    {
        timeRemaining = new NetworkVariable<float>(matchDurationSeconds, NetworkVariableReadPermission.Everyone);
    }

    private void Start()
    {
        text.text = "Match not started. The host must press [ENTER] or [R3] to start this match!";
    }

    private void Update()
    {
        if (FindObjectOfType<NetworkMatchManager>().isMatchStarted.Value && IsHost)
        {
            timeRemaining.Value -= Time.deltaTime;
            if(timeRemaining.Value <= 0)
                FindObjectOfType<NetworkMatchManager>().EndMatch();
        }
        if(FindObjectOfType<NetworkMatchManager>().isMatchStarted.Value)
            text.text = "Time remaining: " + (int)timeRemaining.Value + " seconds";
    }
}

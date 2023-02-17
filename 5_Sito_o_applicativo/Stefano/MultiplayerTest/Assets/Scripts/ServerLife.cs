using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ServerLife : NetworkBehaviour
{
    private NetworkVariable<float> timePast;
    public TextMeshPro text;

    private void Start()
    {
        timePast = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone);
    }

    private void Update()
    {
        if(IsHost)
            timePast.Value += Time.deltaTime;
        text.text = "Server Life: " + (int)timePast.Value + " seconds";
    }
}

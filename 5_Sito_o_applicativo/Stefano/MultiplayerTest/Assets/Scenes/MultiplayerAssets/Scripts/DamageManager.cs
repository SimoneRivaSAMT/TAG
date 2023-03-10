using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class DamageManager : NetworkBehaviour
{
    NetworkVariable<int> p1Life;
    NetworkVariable<int> p2Life;
    NetworkVariable<int> p3Life;
    NetworkVariable<int> p4Life;

    public GameObject lives;

    private void Awake()
    {
        p1Life = new NetworkVariable<int>(3);
        p2Life = new NetworkVariable<int>(3);
        p3Life = new NetworkVariable<int>(3);
        p4Life = new NetworkVariable<int>(3);
        lives = GameObject.FindGameObjectWithTag("player_lives_ui");
        UpdateText();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerHittedServerRpc(ulong playerNetId)
    {
        Debug.LogError("Il player " + playerNetId + " è stato colpito!");
        ulong pid = 1;
        if(playerNetId > 1)
            pid = playerNetId - (playerNetId - 2);
        switch (pid)
        {
            case 1:
                p1Life.Value = p1Life.Value - 1;
                break;
            case 2:
                p2Life.Value = p2Life.Value - 1;
                break;
            case 3:
                p3Life.Value = p3Life.Value - 1;
                break;
            case 4:
                p4Life.Value = p4Life.Value - 1;
                break;
        }
        UpdateText();
        Debug.Log(
            "Player 1: " + p1Life.Value + "\n" +
            "Player 2: " + p2Life.Value + "\n" +
            "Player 3: " + p3Life.Value + "\n" +
            "Player 4: " + p4Life.Value
        );
    }

    private void UpdateText()
    {
        //lives.GetComponentsInChildren<TMP_Text>()[0].text = p1Life.Value.ToString();
        //lives.GetComponentsInChildren<TMP_Text>()[1].text = p2Life.Value.ToString();
        //lives.GetComponentsInChildren<TMP_Text>()[2].text = p3Life.Value.ToString();
        //lives.GetComponentsInChildren<TMP_Text>()[3].text = p4Life.Value.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 Questo script gestisce le cutscene su tutti i giocatori
 L'host da il comando e tutti i client seguono
 */

public class CutsceneManager : NetworkBehaviour
{
    private Animator anim;
    private float tmp_deltaTime = 0f;
    private bool tmp_repeatAction = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && IsHost) //Se chi richiama questa funzione è host allora starta la cutscene
        {
            StartAnimationClientRpc();
        }
    }

    [ClientRpc]
    public void StartAnimationClientRpc()
    {
        GetComponent<Camera>().enabled = true;
        anim.SetTrigger("start");
        StartCoroutine(DisableCamAfterSeconds(3f));
    }

    private IEnumerator DisableCamAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        GetComponent<Camera>().enabled = false;
    }
}



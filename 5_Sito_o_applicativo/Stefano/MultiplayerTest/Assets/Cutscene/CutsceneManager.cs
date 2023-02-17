using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
        if (Input.GetKeyDown(KeyCode.C) && IsHost)
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



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    private bool doorOpen;

    public float time = 2;
    public GameObject door;
    public Material greenGlow;
    public Material redGlow;

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        if (doorOpen)
        {
            GetComponent<MeshRenderer>().material = greenGlow;
            StartCoroutine(CloseDoor());
        }
        else
            GetComponent<MeshRenderer>().material = redGlow;
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(time);
        door.GetComponent<Animator>().SetBool("IsOpen", false);
        doorOpen = false;
        GetComponent<MeshRenderer>().material = redGlow;
        yield break;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static bool found = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            found = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        found = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public GameObject player;
    private bool lookAt = false;
    // Update is called once per frame
    void Update()
    {
        if (PlayerDetection.found)
        {
            // Look at the player
            lookAt = true;

            print("found");
        }

        if (lookAt)
        {
            transform.LookAt(player.transform);
        }
    }
}

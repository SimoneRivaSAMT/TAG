using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Contenitore degli oggetti da spawnare. potrebbe non servire
 */
public class GameObjectsContainer : MonoBehaviour
{
    public GameObject cube1;

    public GameObject GetObject(int id)
    {
        switch (id)
        {
            case 1:
                return cube1;
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

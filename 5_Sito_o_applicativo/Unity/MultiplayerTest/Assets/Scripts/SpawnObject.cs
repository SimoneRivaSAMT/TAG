using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnObject : NetworkBehaviour
{
    public void SpawnGameObject(GameObject prefab, Vector3 location, Quaternion rotation)
    {
        if (!IsHost)
            return;
        GameObject objToSpawn = Instantiate(prefab, location, rotation);
        objToSpawn.GetComponent<NetworkObject>().Spawn();
    }
}

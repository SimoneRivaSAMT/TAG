using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnObject : NetworkBehaviour
{
    private GameObjectsContainer gameObjectsContainer;
    private void Start()
    {
        gameObjectsContainer = FindObjectOfType<GameObjectsContainer>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnGameObjectServerRpc(int gameObjectId, float posX, float posY, float posZ)
    {
        Vector3 pos = new Vector3(posX, posY, posZ);

        GameObject objToSpawn = Instantiate(gameObjectsContainer.GetObject(gameObjectId),
            pos,
            Quaternion.identity);
        objToSpawn.GetComponent<NetworkObject>().Spawn();
    }
}

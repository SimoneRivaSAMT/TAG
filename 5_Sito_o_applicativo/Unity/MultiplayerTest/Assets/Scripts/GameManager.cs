using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Runtime.InteropServices;

namespace Assets.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public GameObject spawn1;
        public Transform spawn1_loc;
        private SpawnObject spawnObject;


        private void Start()
        {
            spawnObject = FindObjectOfType<SpawnObject>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 pos = new(spawn1_loc.position.x, spawn1_loc.position.y, spawn1_loc.position.z);
                spawnObject.SpawnGameObject(spawn1, pos, Quaternion.identity);
            }
        }
    }
}

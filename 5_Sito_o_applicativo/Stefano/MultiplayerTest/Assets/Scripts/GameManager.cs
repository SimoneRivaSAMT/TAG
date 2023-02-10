using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Runtime.InteropServices;
using TMPro;
using System.Linq;

namespace Assets.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private SpawnObject spawnObject;
        private PlayersManagement playersManagement;

        public Transform spawn1_loc;
        public TextMeshProUGUI playerCountText; 

        private void Start()
        {
            spawnObject = FindObjectOfType<SpawnObject>();
            playersManagement = FindObjectOfType<PlayersManagement>();
        }

        private void Update()
        {
            playerCountText.text = "Players: " + playersManagement.GetNumberOfClients();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawnObject.SpawnGameObjectServerRpc(1, spawn1_loc.position.x,
                    spawn1_loc.position.y,
                    spawn1_loc.position.z);

            }
        }
    }
}

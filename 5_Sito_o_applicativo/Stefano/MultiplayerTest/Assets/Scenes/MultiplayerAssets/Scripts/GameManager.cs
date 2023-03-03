using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Runtime.InteropServices;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private SpawnObject spawnObject;
        private PlayersManagement playersManagement;

        public Transform spawn1_loc;
        public Transform playerRespawnLocation;
        public TextMeshProUGUI playerCountText;
        public GameObject console;

        private float deltaTime = 0f;

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
            if (Input.GetKey(KeyCode.F1))
            {
                console.active = !console.active;
                Cursor.visible = !Cursor.visible;
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                deltaTime += Time.deltaTime;
                if(deltaTime > 2f)
                {
                    deltaTime = 0f;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    NetworkManager.Singleton.Shutdown();
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}

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
        private SpawnObject spawnObject; //server per spawnare oggetti. potrebbe non servirci
        private PlayersManagement playersManagement;

        public Transform spawn1_loc; //coordinate di spawn dell'oggetto. potrebbe non servirci
        public Transform playerRespawnLocation; //dove il player respawna. da implementare
        public TextMeshProUGUI playerCountText;
        public GameObject console; //console di debug. da togliere alla fine del progetto

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
                //metodo che spawna un oggetto. potrebbe non servire
                spawnObject.SpawnGameObjectServerRpc(1, spawn1_loc.position.x,
                    spawn1_loc.position.y,
                    spawn1_loc.position.z);

            }
            if (Input.GetKey(KeyCode.F1)) //disabilito / abilito la console
            {
                console.active = !console.active;
                Cursor.visible = !Cursor.visible;
            }
            


        }
    }
}

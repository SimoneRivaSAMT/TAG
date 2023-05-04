using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;
using System;
using System.Linq;
using Assets.CasualMap;

/*
 Questo script server a generare la mappa casualmente. Si attiva premendo [M]
 */

public class MapGenerator : NetworkBehaviour
{
    [Header("Wall Vars")]
    public int WALL_COUNT = 6; //Numero di cubi
    public int PROBABILITY = 25; //Probabilità di alzata
    public float deltaH; //Quanto di deve alzare
    bool[] walls; //Array che determina le mura (true = alto, false = basso)
    bool[] wallsBefore; //Array che ha lo stato delle mura precedente rispetto a quello attuale
    GameObject[] wallsPhysic; //array di game objects che contiene i cubi fisici
    NetworkVariable<FixedString512Bytes> serializedWalls; //supporto, NON TOCCARE
    NetworkVariable<FixedString512Bytes> serializedWallsBefore; //supporto, NON TOCCARE

    [Header("Colors")]
    public Material green;
    public Material yellow;
    public Material orange;
    public Material red;
    public Material normal;

    [Header("Textes")]
    public TextMeshProUGUI wallInfoDebug; //solo a lvello di debug


    private void Awake()
    {
        serializedWalls = new NetworkVariable<FixedString512Bytes>(); //istanzio la NetworkVarible come stringa fissa a 512 byte
        serializedWallsBefore = new NetworkVariable<FixedString512Bytes>();  //istanzio la NetworkVarible come stringa fissa a 512 byte
    }
    private void Start()
    {   
        walls = new bool[WALL_COUNT];
        wallsBefore = new bool[WALL_COUNT];
        wallsPhysic = GameObject.FindGameObjectsWithTag("go-matrix"); //ogni muro DEVE avere questo TAG
        wallsPhysic = wallsPhysic.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray(); //ordino
        StartCoroutine(AutoChageMap());
    }


    [ServerRpc(RequireOwnership = false)]
    public void GenerateMapServerRpc()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            wallsBefore[i] = walls[i]; //copio l'array precedente
            walls[i] = UnityEngine.Random.Range(0, 100) < PROBABILITY; //genero la nuova mappa
        }
        serializedWalls.Value = ArrayToString(walls, ','); //serializzo l'array nella net var
        serializedWallsBefore.Value = ArrayToString(wallsBefore, ','); //serializzo l'array nella net var
        StartCoroutine(SendCommandToClientCooldown()); //attesa
    }


    [ClientRpc]
    private void ApplyConfigurationClientRpc()
    {
        string[] serializedWallsArray = serializedWalls.Value.ToString().Split(",");
        string[] serializedWallsBeforeArray = serializedWallsBefore.Value.ToString().Split(",");
        walls = new bool[WALL_COUNT];
        wallsBefore = new bool[WALL_COUNT];
        wallInfoDebug.text = "ClientWalls:" + "\r\n" + serializedWalls.Value; //debug
        for (int i = 0; i < walls.Length; i++) //vado a riformare l'array di bool dalla stringa serializzata
        {
            if (serializedWallsArray[i].Equals("True"))
            {
                walls[i] = true;
            }
            else
            {
                walls[i] = false;
            }
            if (serializedWallsBeforeArray[i].Equals("True"))
            {
                wallsBefore[i] = true;
            }
            else
            {
                wallsBefore[i] = false;
            }
        }
        for (int i = 0; i < walls.Length; i++) //controllo lo stato precedente e decido se abbassare o alzare il muro
        {
            if (walls[i] == wallsBefore[i])
            {
                continue;
            }
            if(walls[i] && !wallsBefore[i])
            {
                StartCoroutine(TranslateObject(wallsPhysic[i], .2f, Vector3.zero, 0, deltaH, 0));
            }
            else
            {
                StartCoroutine(TranslateObject(wallsPhysic[i], .2f, Vector3.zero, 0, -deltaH, 0));
            }
        }
    }

    private void ChangeMaterial(int id, GameObject gameObject)
    {
        switch (id)
        {
            case 0:
                gameObject.GetComponent<Renderer>().material = green;
                break;
            case 1:
                gameObject.GetComponent<Renderer>().material = yellow;
                break;
            case 2:
                gameObject.GetComponent<Renderer>().material = orange;
                break;
            case 3:
                gameObject.GetComponent<Renderer>().material = red;
                break;
            case 4:
                gameObject.GetComponent<Renderer>().material = normal;
                break;
        }
    }

    private string ArrayToString(bool[] array, char separator)
    {
        string res = "";
        for (int i = 0; i < array.Length; i++)
        {
            if(i != array.Length - 1)
                res += array[i].ToString() + separator;
            else
                res += array[i].ToString();
        }
        return res;
    }

    private IEnumerator SendCommandToClientCooldown()
    {
        if (IsHost)
        {
            yield return new WaitForSeconds(.5f);
            ApplyConfigurationClientRpc();
        }
    }

    //Metodo che fa il translate indipendentemente dal metodo Update()
    private IEnumerator TranslateObject(GameObject obj, float time, Vector3 velocity, float deltaX = 0, float deltaY = 0, float deltaZ = 0)
    {
        ChangeMaterial((int)IdToColor.green, obj);
        yield return new WaitForSeconds(1);
        ChangeMaterial((int)IdToColor.yellow, obj);
        yield return new WaitForSeconds(2);
        ChangeMaterial((int)IdToColor.orange, obj);
        yield return new WaitForSeconds(3);
        ChangeMaterial((int)IdToColor.red, obj);
        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = new Vector3(startPos.x + deltaX, startPos.y + deltaY, startPos.z + deltaZ);
        Vector3 currentPos = startPos;
        while (currentPos != targetPos)
        {
            currentPos = obj.transform.position;
            obj.transform.position = Vector3.SmoothDamp(currentPos, targetPos, ref velocity, time);
            yield return new WaitForSeconds(.005f);
        }
        ChangeMaterial((int)IdToColor.normal, obj);
    }

    private IEnumerator AutoChageMap() //Genera la mappa ogni tot secondi
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            GenerateMapServerRpc();
        }
    }

    enum IdToColor //enumeratore locale per i colori (green = 0, yellow = 1, ...)
    {
        green,
        yellow,
        orange,
        red,
        normal
    }
}

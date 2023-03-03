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

public class MapGenerator : NetworkBehaviour
{
    [Header("Wall Vars")]
    public int WALL_COUNT = 6;
    public int PROBABILITY = 25;
    public float deltaH = .7f;
    int generationNumber = 0;
    bool[] walls;
    bool[] wallsBefore;
    GameObject[] wallsPhysic;
    NetworkVariable<FixedString512Bytes> serializedWalls;
    NetworkVariable<FixedString512Bytes> serializedWallsBefore;

    [Header("Colors")]
    public Material green;
    public Material yellow;
    public Material orange;
    public Material red;
    public Material normal;

    [Header("Textes")]
    public TextMeshProUGUI wallInfoDebug;


    private void Awake()
    {
        serializedWalls = new NetworkVariable<FixedString512Bytes>();
        serializedWallsBefore = new NetworkVariable<FixedString512Bytes>();
    }
    private void Start()
    {   
        walls = new bool[WALL_COUNT];
        wallsBefore = new bool[WALL_COUNT];
        wallsPhysic = GameObject.FindGameObjectsWithTag("go-matrix");
        wallsPhysic = wallsPhysic.OrderBy(obj => obj.name, new AlphanumComparatorFast()).ToArray();
    }


    [ServerRpc(RequireOwnership = false)]
    public void GenerateMapServerRpc()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            wallsBefore[i] = walls[i];
            walls[i] = UnityEngine.Random.Range(0, 100) < PROBABILITY;
        }
        serializedWalls.Value = ArrayToString(walls, ',');
        serializedWallsBefore.Value = ArrayToString(wallsBefore, ',');
        StartCoroutine(SendCommandToClientCooldown());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            GenerateMapServerRpc();
        }
            
    }

    [ClientRpc]
    private void ApplyConfigurationClientRpc()
    {
        string[] serializedWallsArray = serializedWalls.Value.ToString().Split(",");
        string[] serializedWallsBeforeArray = serializedWallsBefore.Value.ToString().Split(",");
        walls = new bool[WALL_COUNT];
        wallsBefore = new bool[WALL_COUNT];
        wallInfoDebug.text = "ClientWalls:" + "\r\n" + serializedWalls.Value;
        for (int i = 0; i < walls.Length; i++)
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
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == wallsBefore[i])
            {
                continue;
            }
            if(walls[i] && !wallsBefore[i])
            {
                StartCoroutine(TranslateObject(wallsPhysic[i], .2f, Vector3.zero, 0, .7f, 0));
            }
            else
            {
                StartCoroutine(TranslateObject(wallsPhysic[i], .2f, Vector3.zero, 0, -.7f, 0));
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

    private IEnumerator AutoChageMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            GenerateMapServerRpc();
        }
    }

    enum IdToColor
    {
        green,
        yellow,
        orange,
        red,
        normal
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.Netcode;

public class MapGenerator : NetworkBehaviour
{
    public int WALL_COUNT = 6;
    public int PROBABILITY = 25;
    public float deltaH = .7f;
    int generationNumber = 0;
    bool[] walls;
    bool[] wallsBefore;

    private void Start()
    {
        walls = new bool[WALL_COUNT];
        wallsBefore = new bool[WALL_COUNT];
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = Random.Range(0, 100) > PROBABILITY;
        }
        walls = wallsBefore;
    }

    public void RegenerateMap()
    {
        wallsBefore = walls;
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = Random.Range(0, 100) > PROBABILITY;
        }
        ApplyConfiguration();
    }

    private void ApplyConfiguration()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == wallsBefore[i])
                return;
            if(walls[i] && !wallsBefore[i])
            {
                Transform actualPos = GetComponent<Transform>();
                Vector3 actualPosV3 = new(actualPos.position.x, actualPos.position.y, actualPos.position.z);
                Vector3 toPosV3 = new(actualPos.position.x, actualPos.position.y, actualPos.position.z);
            }
        }
    }

    private IEnumerator TranslateObject(Vector3 from, Vector3 to)
    {
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private static NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }
    //public Transform Target;

    //Just for debugging purposes
    [SerializeField]
    private string currentState;
    //public float remainingDistance = 8f;

    [HideInInspector]
    public static GameObject Target;

    public Path path;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        //stateMachine.Initialise(); // 
        Target = GetClosestPlayer();
        agent.destination = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Target = GetClosestPlayer();
        agent.destination = Target.transform.position;
        Debug.Log(name + "Pos: " + Target.name + ": " + agent.destination);
        /*if (agent.remainingDistance > remainingDistance)
        {
            //agent.updateRotation = false;
            Debug.Log(name + "Pos: " + Target.name + ": " + agent.destination);
            agent.isStopped = false;
        }*/
        /*else if (agent.remainingDistance < 5)
        {
            agent.destination = GetClosestPlayer().transform.position;
            //agent.updateRotation = false;
            float xOppositeDestination = Mathf.Abs(agent.transform.position.x - GetClosestPlayer().transform.position.x);
            float zOppositeDestination = Mathf.Abs(agent.transform.position.z - GetClosestPlayer().transform.position.z);
            xOppositeDestination += agent.transform.position.x;
            zOppositeDestination += agent.transform.position.z;
            agent.destination = new Vector3(xOppositeDestination, GetClosestPlayer().transform.position.y, zOppositeDestination);
            agent.isStopped = false;
        }*/
        /*else
        {
            agent.isStopped = true;
        }*/
            
        
        
        // Re bakes everything
        //UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    public GameObject GetClosestPlayer()
    {
        List<GameObject> players = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            players.Add(enemy);
        }

        float agentPosition = Mathf.Abs(agent.transform.position.x) + Mathf.Abs(agent.transform.position.z);
        float previousDistance = 0;
        GameObject closestPlayer = null;
        for (int i = 0; i < players.Count; i++)
        {
            //Debug.Log(players[i].name + ": " + players[i].transform.position.ToString());
            if (players[i].transform.position != agent.transform.position && i != 0)
            {
                float playerPosition = Mathf.Abs(players[i].transform.position.x) + Mathf.Abs(players[i].transform.position.z);
                float remainingDistance = Mathf.Abs(playerPosition - agentPosition);
                if (remainingDistance < previousDistance)
                {
                    previousDistance = remainingDistance;
                    closestPlayer = players[i];
                }
            }else if(i == 0)
            {
                previousDistance = Mathf.Abs(players[i].transform.position.x) + Mathf.Abs(players[i].transform.position.z);
                closestPlayer = players[i];
            }
        }
        return closestPlayer;
    }
}

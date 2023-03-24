using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    //public Transform Target;

    //Just for debugging purposes
    [SerializeField]
    private string currentState;
    //public float remainingDistance = 8f;

    [HideInInspector]
    public GameObject Target;

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
    }

    public GameObject GetClosestPlayer()
    {
        // Create list of all Player and AI Enemies
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
        // Check all players/AIs
        for (int i = 0; i < players.Count; i++)
        {
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

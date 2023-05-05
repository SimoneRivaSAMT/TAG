using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerManager playerManager;
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    [HideInInspector]
    public GameObject Target;

    private void Awake()
    {
        playerManager = new PlayerManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Target = GetClosestPlayer();
        agent.destination = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Target = GetClosestPlayer();
        agent.destination = Target.transform.position;
    }

    // AI looks for the closest player / enemy
    public GameObject GetClosestPlayer()
    {
        List<GameObject> players = playerManager.GetPlayers();

        // Simple calculation to transform a position to a relative number
        float agentPosition = Mathf.Abs(agent.transform.position.x) + Mathf.Abs(agent.transform.position.z);
        float previousDistance = 0;
        GameObject closestPlayer = null;

        // Check all players/AIs
        for (int i = 0; i < players.Count; i++)
        {
            // Other player (not itself)
            if (players[i].transform.position != agent.transform.position && i != 0)
            {
                float playerPosition = Mathf.Abs(players[i].transform.position.x) + Mathf.Abs(players[i].transform.position.z);

                // Confront agent with other player / enemy
                float remainingDistance = Mathf.Abs(playerPosition - agentPosition);

                // Check who is closest
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }
    public Transform Target;

    //Just for debugging purposes
    [SerializeField]
    private string currentState;

    public Path path;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        //stateMachine.Initialise(); // 
        agent.destination = Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Target.position;
        if (agent.remainingDistance > 5)
            agent.isStopped = false;
        else
            agent.isStopped = true;
        
        
        // Re bakes everything
        //UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
}

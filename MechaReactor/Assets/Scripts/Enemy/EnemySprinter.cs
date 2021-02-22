using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySprinter : MonoBehaviour
{
    public Transform target;
    // Give enemy units a "Nav Mesh Agent" component
    private NavMeshAgent agent;

    void Start()
    {
        // This code block needs to go in EVERY start function of a pathfinding agent.
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Self explanatory, but set the agent's destination
        agent.SetDestination(target.position); 
    }
}

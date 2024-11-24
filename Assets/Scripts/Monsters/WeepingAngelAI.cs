using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngelAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent navMeshAgent;
    public float detectionAngle = 60f;
    public float detectionDistance = 10f;
    public float movementSpeed = 3.5f;

    private Renderer monsterRenderer;

    void Start()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        monsterRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (IsPlayerLookingAtMonster())
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position); 
        }
    }

    bool IsPlayerLookingAtMonster()
    {
        Vector3 directionToMonster = (transform.position - player.position).normalized;

        float angle = Vector3.Angle(player.forward, directionToMonster);

        float distanceToMonster = Vector3.Distance(player.position, transform.position);

        bool withinAngle = angle < detectionAngle / 2f;
        bool withinDistance = distanceToMonster <= detectionDistance;

        return withinAngle && withinDistance;
    }
}

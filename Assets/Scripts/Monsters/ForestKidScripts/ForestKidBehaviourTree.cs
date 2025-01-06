using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ForestKidBehaviourTree : MonoBehaviour
{
    private ForestKidFSM _fsm;

    private ForestKidFSM.MonsterState _currentState;

    public NavMeshAgent agent;

    public GameObject player;

    [SerializeField] private float detectionRange = 10f;

    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float wanderDelay = 2f;

    private float wanderTimer = 0f;

    private Vector3 spawnPosition;

    private MonsterCombatHandler combatHandler;

    public float audioRange = 20f;


    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        float difficultyMode = PlayerPrefs.GetFloat("DifficultyMode", 1f);
        agent.speed = 7.0f * difficultyMode;

        _fsm = GetComponent<ForestKidFSM>();
        spawnPosition = transform.position;
        combatHandler = GetComponent<MonsterCombatHandler>();
    }

    public void SetCurrentState(ForestKidFSM.MonsterState newState)
    {
        _currentState = newState;
    }

    public void ExecuteBehavior()
    {
        switch (_currentState)
        {
            case ForestKidFSM.MonsterState.DayWander:
                DayWanderBehavior();
                break;

            case ForestKidFSM.MonsterState.NightHostile:
                NightHostileBehavior();
                break;

            case ForestKidFSM.MonsterState.Attacked:
                AttackedBehavior();
                break;
        }
    }

    private void DayWanderBehavior()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(transform.position); 
        }
        else
        {
            PatrolArea();
        }
    }

    private void NightHostileBehavior()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(player.transform.position);

        }
        else
        {
            PatrolArea();
        }
    }

    private void AttackedBehavior()
    {
        if (combatHandler.IsHealthLow())
        {
            agent.SetDestination(spawnPosition);
        }
        else
        {
            if (CanSeePlayer())
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                PatrolArea();
            }
        }
    }

    private void PatrolArea()
    {
        if (wanderTimer <= 0f)
        {
            Vector3 randomDestination = GetRandomPointAroundSpawn();
            agent.SetDestination(randomDestination);
            wanderTimer = wanderDelay;
        }

        wanderTimer -= Time.deltaTime;
    }


    private Vector3 GetRandomPointAroundSpawn()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += spawnPosition;
        randomDirection.y = spawnPosition.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return spawnPosition;
    }

    private bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= detectionRange;
    }

    public bool IsPlayerInAudioRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= audioRange;
    }
}

using UnityEngine;
using UnityEngine.AI;

public class LookAwayMonsterBehaviourTree : MonoBehaviour
{
    private LookAwayMonsterFSM.MonsterState _currentState;

    [Header("References")]
    public GameObject player;
    public NavMeshAgent agent;

    [Header("Look Detection")]
    public float maxViewAngle = 60f;  
    public float dotThreshold;      

    public float DistanceToPlayer { get; private set; }
    public bool IsPlayerLookingAtMonster { get; private set; }

    private void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();

        float halfAngleRadians = maxViewAngle * 0.5f * Mathf.Deg2Rad;
        dotThreshold = Mathf.Cos(halfAngleRadians);
    }

    public void SetCurrentState(LookAwayMonsterFSM.MonsterState newState)
    {
        _currentState = newState;
        if (_currentState == LookAwayMonsterFSM.MonsterState.Seen)
        {
            if (agent)
            {
                agent.SetDestination(transform.position);
            }
        }
    }

    public void ExecuteBehavior()
    {
        UpdateDistanceAndLook();

        switch (_currentState)
        {
            case LookAwayMonsterFSM.MonsterState.Idle:
                IdleBehavior();
                break;
            case LookAwayMonsterFSM.MonsterState.Seen:
                SeenBehavior();
                break;
            case LookAwayMonsterFSM.MonsterState.Hidden:
                HiddenBehavior();
                break;
        }
    }
    private void IdleBehavior()
    {
        if (agent && agent.velocity.sqrMagnitude > 0.1f)
        {
            agent.SetDestination(transform.position);
        }
    }

    private void SeenBehavior()
    {
        if (agent && agent.velocity.sqrMagnitude > 0.1f)
        {
            agent.SetDestination(transform.position);
        }

    }

    private void HiddenBehavior()
    {

        if (!agent || player == null) return;

        agent.speed = 2f;

        Vector3 playerPos = player.transform.position;
        agent.SetDestination(playerPos);
    }

    private void UpdateDistanceAndLook()
    {
        if (!player)
        {
            DistanceToPlayer = Mathf.Infinity;
            IsPlayerLookingAtMonster = false;
            return;
        }

        DistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        Vector3 playerForward = player.transform.forward;
        Vector3 toMonster = (transform.position - player.transform.position).normalized;

        float dot = Vector3.Dot(playerForward, toMonster);
        IsPlayerLookingAtMonster = (dot >= dotThreshold);
    }
}

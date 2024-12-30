using UnityEngine;

public class BlindMonsterFSM : MonoBehaviour
{
    public BlindMonsterBehaviourTree behaviorTree;

    private float nextAttackTime;
    public float attackCooldown = 2f;

    public enum MonsterState
    {
        Idle,
        Chasing
    }

    public MonsterState currentState = MonsterState.Idle;

    public Animator animator;

    private void Start()
    {
        behaviorTree = GetComponent<BlindMonsterBehaviourTree>();
    }

    private void Update()
    {
        StateMachineLogic();
        UpdateAnimations();

        behaviorTree.ExecuteBehavior();
    }

    private void StateMachineLogic()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                if (behaviorTree.CanHearPlayer && !behaviorTree.playerController.stealth)
                {
                    currentState = MonsterState.Chasing;

                    behaviorTree.lastHeardPlayerPosition = behaviorTree.GetPlayerPosition();
                }
                break;

            case MonsterState.Chasing:
                if (behaviorTree.CanHearPlayer)
                {
                    behaviorTree.lastHeardPlayerPosition = behaviorTree.GetPlayerPosition();
                }

                if (behaviorTree.HasReachedLastKnownPosition())
                {
                    if (!behaviorTree.CanHearPlayer)
                    {
                        currentState = MonsterState.Idle;
                    }
                }
            break;
        }

        behaviorTree.SetCurrentState(currentState);
    }

    private void UpdateAnimations()
    {
        bool isChasingState = (currentState == MonsterState.Chasing);

        float sqrSpeed = behaviorTree.agent.velocity.sqrMagnitude;
        bool isMoving = sqrSpeed > 0.1f;

        animator.SetBool("Chasing", isChasingState && isMoving);

        float distanceToPlayer = Vector3.Distance(transform.position, behaviorTree.player.transform.position);
        Debug.Log(distanceToPlayer);
        if (Time.time >= nextAttackTime && distanceToPlayer < 3f)
        {
            Debug.Log("YESS");
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}

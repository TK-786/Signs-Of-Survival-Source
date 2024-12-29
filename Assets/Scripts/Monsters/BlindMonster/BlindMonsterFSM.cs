using UnityEngine;

public class BlindMonsterFSM : MonoBehaviour
{
    public BlindMonsterBehaviourTree behaviorTree;

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

        SetAnimation();

        behaviorTree.ExecuteBehavior();
    }

    private void StateMachineLogic()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                if (behaviorTree.CanHearPlayer)
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

    private void SetAnimation()
    {
        if (animator == null) return;

        // "Hostile" = true if Chasing, false if Idle
        bool isHostile = (currentState == MonsterState.Chasing);
        animator.SetBool("Hostile", isHostile);
    }
}

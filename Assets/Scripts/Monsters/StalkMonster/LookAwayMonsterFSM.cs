using UnityEngine;

public class LookAwayMonsterFSM : MonoBehaviour
{
    public LookAwayMonsterBehaviourTree behaviorTree;

    public enum MonsterState
    {
        Idle, 
        Seen,  
        Hidden  
    }

    public MonsterState currentState = MonsterState.Idle;

    public float rangeThreshold = 50f;

    private void Start()
    {
        behaviorTree = GetComponent<LookAwayMonsterBehaviourTree>();
    }

    private void Update()
    {
        StateMachineLogic();

        behaviorTree.ExecuteBehavior();
    }

    private void StateMachineLogic()
    {
        float distanceToPlayer = behaviorTree.DistanceToPlayer;
        bool isPlayerLooking = behaviorTree.IsPlayerLookingAtMonster;

        if (distanceToPlayer > rangeThreshold)
        {
            currentState = MonsterState.Idle;
        }
        else
        {
            if (isPlayerLooking)
            {
                currentState = MonsterState.Seen;
            }
            else
            {
                currentState = MonsterState.Hidden;
            }
        }


        behaviorTree.SetCurrentState(currentState);
        
    }
}

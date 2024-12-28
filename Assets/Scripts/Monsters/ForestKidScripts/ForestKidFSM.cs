using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestKidFSM : MonoBehaviour
{
    public ForestKidBehaviourTree behaviorTree;

    public enum MonsterState
    {
        DayWander,
        NightHostile,
        Attacked
    }

    public MonsterState currentState = MonsterState.DayWander;

    private bool isNight = false;
    private bool isAttacked = false;

    private MonsterCombatHandler combatHandler;

    public Animator animator;

    private void Start()
    {
        behaviorTree = GetComponent<ForestKidBehaviourTree>();
        combatHandler = GetComponent<MonsterCombatHandler>();
    }

    private void Update()
    {
        CheckConditions();

        StateMachineLogic();

        setAnimation();

        behaviorTree.ExecuteBehavior();
    }

    private void CheckConditions()
    {
        isNight = TickManager.Night;

        isAttacked = combatHandler.TakenDMG();
    }

    private void StateMachineLogic()
    {
        switch (currentState)
        {
            case MonsterState.DayWander:
                if (isNight) 
                {
                    currentState = MonsterState.NightHostile;
                }

                else if (isAttacked)
                {
                    currentState = MonsterState.Attacked;
                }
                break;

            case MonsterState.NightHostile:
                if (!isNight)
                {
                    currentState = MonsterState.DayWander;
                }
                else if (isAttacked)
                {
                    currentState = MonsterState.Attacked;
                }
                break;

            case MonsterState.Attacked:
                if (!isAttacked)
                {
                    currentState = isNight ? MonsterState.NightHostile : MonsterState.DayWander;
                }
                break;
        }

        behaviorTree.SetCurrentState(currentState);
    }

    public void setAnimation() {
        bool isHostile = (currentState == MonsterState.NightHostile || currentState == MonsterState.Attacked);

        animator.SetBool("Hostile", isHostile);
    }
}

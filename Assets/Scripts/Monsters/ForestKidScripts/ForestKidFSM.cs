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

    public float audioRange = 20f;

    private AudioSource audioSource;

    private void Start()
    {
        behaviorTree = GetComponent<ForestKidBehaviourTree>();
        combatHandler = GetComponent<MonsterCombatHandler>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckConditions();

        StateMachineLogic();

        setAnimation();

        SetAudioPitch();

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
                if (isAttacked)
                {
                    currentState = MonsterState.Attacked;
                }
                else if (!isNight)
                {
                    currentState = MonsterState.DayWander;
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

    private void SetAudioPitch()
    {
        if (behaviorTree.IsPlayerInAudioRange())
        {
            if (currentState == MonsterState.NightHostile || currentState == MonsterState.Attacked)
            {
                audioSource.pitch = 2.0f;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.pitch = 1.0f;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

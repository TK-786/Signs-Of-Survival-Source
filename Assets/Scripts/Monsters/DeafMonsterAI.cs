using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeafMonsterAI : MonoBehaviour
{
    public Transform player;
    public float hearingRadius = 10f;
    public NavMeshAgent navMeshAgent;

    void Update()
    {
        if (IsPlayerMakingNoise())
        {
            ChasePlayer();
        }
    }

    bool IsPlayerMakingNoise()
    {
        AudioSource playerAudio = player.GetComponent<AudioSource>();

        if (playerAudio != null && playerAudio.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            return distance <= hearingRadius;
        }

        return false;
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
}

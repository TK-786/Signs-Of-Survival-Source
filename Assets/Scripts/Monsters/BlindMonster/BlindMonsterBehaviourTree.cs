using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Behavior “Tree” that handles Idle vs. Chasing logic with a "last known position".
/// </summary>
public class BlindMonsterBehaviourTree : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public AudioSource audioSource;

    private BlindMonsterFSM.MonsterState _currentState;

    public Vector3 lastHeardPlayerPosition { get; set; }

    public float hearingRadius = 19.5f;
    public float arriveThreshold = 1.0f;

    public AudioClip spotPlayerSound;
    public AudioClip chaseSound;
    public AudioClip idleDefaultSound;

    private bool hasPlayedSpotSound;
    private bool isSpotSoundPlaying;

    public float idleHearingRadius = 50f;
    private bool isIdleSoundPlaying;

    public bool CanHearPlayer { get; private set; }
    public PlayerController playerController;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        float difficultyMode = PlayerPrefs.GetFloat("DifficultyMode", 1f);
        agent.speed = 7.0f * difficultyMode;
    }

    public void SetCurrentState(BlindMonsterFSM.MonsterState newState)
    {
        _currentState = newState;

        if (_currentState == BlindMonsterFSM.MonsterState.Idle)
        {
            StopChaseSounds();
            PlayIdleDefaultSound();
        }
    }

    public void ExecuteBehavior()
    {
        UpdateHearing();

        switch (_currentState)
        {
            case BlindMonsterFSM.MonsterState.Idle:
                IdleBehavior();
                break;

            case BlindMonsterFSM.MonsterState.Chasing:
                ChasingBehavior();
                break;
        }
    }

    private void IdleBehavior()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            agent.SetDestination(transform.position);
        }

        if (isSpotSoundPlaying || hasPlayedSpotSound)
        {
            isSpotSoundPlaying = false;
            hasPlayedSpotSound = false;
        }

        if (player && Vector3.Distance(transform.position, player.transform.position) <= idleHearingRadius)
        {
            PlayIdleDefaultSound();
        }
        else
        {
            StopIdleDefaultSound();
        }
    }

    private void ChasingBehavior()
    {
        if (!hasPlayedSpotSound && spotPlayerSound != null)
        {
            if (audioSource && audioSource.isPlaying) audioSource.Stop();
            audioSource?.PlayOneShot(spotPlayerSound);

            isSpotSoundPlaying = true;
            hasPlayedSpotSound = true;
        }
        else
        {
            if (isSpotSoundPlaying && audioSource && !audioSource.isPlaying)
            {
                isSpotSoundPlaying = false;
            }
            if (!isSpotSoundPlaying)
            {
                PlayChaseSound();
            }
        }

        agent.SetDestination(lastHeardPlayerPosition);
    }

    private void UpdateHearing()
    {
        CanHearPlayer = false;

        AudioSource playerAudio = player.GetComponent<AudioSource>();
        if (playerAudio && playerAudio.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= hearingRadius)
            {
                CanHearPlayer = true;
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    public bool HasReachedLastKnownPosition()
    {
        float distance = Vector3.Distance(transform.position, lastHeardPlayerPosition);
        bool isStationary = agent.velocity.sqrMagnitude < 1f;
        return (distance <= arriveThreshold) || isStationary;
    }

    private void PlayChaseSound()
    {
        if (!audioSource || !chaseSound) return;

        if (!audioSource.isPlaying || audioSource.clip != chaseSound)
        {
            audioSource.clip = chaseSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopChaseSounds()
    {
        if (audioSource && audioSource.isPlaying && audioSource.clip == chaseSound)
        {
            audioSource.Stop();
        }
    }

    private void PlayIdleDefaultSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != idleDefaultSound)
        {
            audioSource.clip = idleDefaultSound;
            audioSource.loop = true;
            audioSource.Play();
            isIdleSoundPlaying = true;
        }
    }

    private void StopIdleDefaultSound()
    {
        if (audioSource && audioSource.isPlaying && audioSource.clip == idleDefaultSound)
        {
            audioSource.Stop();
            isIdleSoundPlaying = false;
        }
    }
}

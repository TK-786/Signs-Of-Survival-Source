using UnityEngine;
using UnityEngine.AI;

public class BlindMonsterAI : MonoBehaviour
{
    public float hearingRadius = 19.5f;
    public float defaultSoundRadius = 50f;
    public NavMeshAgent navMeshAgent;
    public LayerMask groundLayer;

    public AudioClip defaultSound;
    public AudioClip spotPlayerSound;
    public AudioClip chaseSound;
    public AudioClip attackSound;

    public AudioSource audioSource;

    private bool isChasing = false;

    public Camera mainCamera;         // Used for AI logic if necessary
    public GameObject player;
    private PlayerController playerController;

    private Vector3 cameraOffset;     // If you need it for AI logic
    public float cameraHeight = 1.75f;

    // Reference to your new combat/damage script
    private MonsterCombatHandler combatHandler;

    private bool hasPlayedSpotSound = false;
    private bool isSpotSoundPlaying = false;

    private Vector3 lastPosition;
    private Vector3 initialPosition;

    private void Start()
    {
        // Get references
        playerController = player.GetComponent<PlayerController>();
        navMeshAgent.speed = 10f;
        lastPosition = transform.position;
        initialPosition = transform.position;

        combatHandler = GetComponent<MonsterCombatHandler>();
        if (mainCamera != null)
        {
            cameraOffset = new Vector3(0, cameraHeight, 0);
        }
    }

    private void Update()
    {
        HandleDefaultSound();

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // If player is too far away, reset monster and stop any chase audio
        if (distance >= defaultSoundRadius)
        {
            ResetMonster();
            audioSource.Stop();
        }

        // Blind monster chases if player is making noise
        if (playerController.stealth == false && (IsPlayerMakingNoise() || navMeshAgent.velocity.magnitude != 0))
        {
            if (!isChasing && !hasPlayedSpotSound)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                audioSource.PlayOneShot(spotPlayerSound);
                isSpotSoundPlaying = true;
                hasPlayedSpotSound = true;
                isChasing = true;
            }

            // Once the spotPlayerSound finishes, switch to chase loop
            if (isSpotSoundPlaying && !audioSource.isPlaying)
            {
                isSpotSoundPlaying = false;
            }
            if (!isSpotSoundPlaying)
            {
                PlayChaseSound();
            }

            // Actually chase the player
            if (IsPlayerMakingNoise())
            {
                ChasePlayer();
            }
        }
        else
        {
            isChasing = false;
            HandleDefaultSound();
        }
    }

    private void ResetMonster()
    {
        // Return the monster to its starting position
        transform.position = initialPosition;

        // Stop movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private bool IsPlayerMakingNoise()
    {
        AudioSource playerAudio = player.GetComponent<AudioSource>();
        if (playerAudio != null && playerAudio.isPlaying)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= hearingRadius;
        }
        return false;
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }

    private void HandleDefaultSound()
    {
        if (!isChasing && audioSource != null && defaultSound != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= defaultSoundRadius)
            {
                if (!audioSource.isPlaying || audioSource.clip != defaultSound)
                {
                    audioSource.clip = defaultSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    private void PlayChaseSound()
    {
        if (audioSource != null && chaseSound != null && !isSpotSoundPlaying)
        {
            if (!audioSource.isPlaying || audioSource.clip != chaseSound)
            {
                audioSource.clip = chaseSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    // If you still need an “attack sound,” you can call it from here
    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}

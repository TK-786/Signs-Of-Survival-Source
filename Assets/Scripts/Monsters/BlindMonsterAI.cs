using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BlindMonsterAI : MonoBehaviour
{
    public float hearingRadius = 19.5f;
    public float defaultSoundRadius = 50f;
    public NavMeshAgent navMeshAgent;
    public float damageAmount = 20f;
    public float knockbackForce = 70f;
    public LayerMask groundLayer;

    public AudioClip defaultSound;
    public AudioClip spotPlayerSound;
    public AudioClip chaseSound;
    public AudioClip attackSound;

    public AudioSource audioSource;

    private bool isChasing = false;

    public Camera mainCamera;
    public GameObject player;
    private PlayerController playerController;

    private Vector3 cameraOffset;
    public float cameraHeight = 1.75f;

    public float Health = 500f;
    public float maxHealth = 500f;

    private bool hasPlayedSpotSound = false;
    private bool isSpotSoundPlaying = false;

    private Vector3 lastPosition;


    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        navMeshAgent.speed = 10f;
        lastPosition = transform.position;

        if (mainCamera != null)
        {
            cameraOffset = new Vector3(0, cameraHeight, 0);
        }
    }

    private void Update()
    {
        HandleDefaultSound();

        if (IsPlayerMakingNoise() || navMeshAgent.velocity.x + navMeshAgent.velocity.y + navMeshAgent.velocity.z != 0)
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

            if (isSpotSoundPlaying && !audioSource.isPlaying)
            {
                isSpotSoundPlaying = false;
            }

            if (!isSpotSoundPlaying)
            {
                PlayChaseSound();
            }
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

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        Debug.Log("Bullet hit new health is : " + Health);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealthRatio()
    {
        return Health / maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamageAndKnockback(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(50);
        }
    }

    private void ApplyDamageAndKnockback(GameObject player)
    {
        // Apply damage to the player if needed
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);
        }

        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
        knockbackDirection.y = 0f;

        StartCoroutine(ApplyKnockback(player, knockbackDirection, knockbackForce, 0.2f));

        PlayAttackSound();
    }

    private IEnumerator ApplyKnockback(GameObject player, Vector3 direction, float force, float duration)
    {
        float initialYPosition = player.transform.position.y;

        direction.Normalize();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float knockbackStep = force * (1 - (elapsedTime / duration));
            Vector3 movement = direction * knockbackStep * Time.deltaTime;

            Vector3 intendedPosition = player.transform.position + movement;

            // Check for collisions
            if (Physics.Raycast(player.transform.position, movement.normalized, out RaycastHit hit, movement.magnitude, ~0, QueryTriggerInteraction.Ignore))
            {
                intendedPosition = hit.point - movement.normalized * 0.1f;
            }

            intendedPosition.y = initialYPosition;

            playerController.UpdatePosition(intendedPosition);

            // Update camera position if necessary
            if (mainCamera != null)
            {
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    intendedPosition + cameraOffset,
                    elapsedTime / duration
                );
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
            else
            {
                if (audioSource.clip == defaultSound)
                {
                    audioSource.Stop();
                }
            }
        }
        else
        {
            if (audioSource.clip == defaultSound && audioSource.isPlaying)
            {
                audioSource.Stop();
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

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BlindMonsterAI : MonoBehaviour
{
    public float hearingRadius = 10f;
    public NavMeshAgent navMeshAgent;
    public float damageAmount = 20f;
    public float knockbackForce = 70f; // Reduced knockback force for small knockback
    public LayerMask groundLayer; // Assign this in the Inspector to the Ground layer

    public AudioClip attackSound;
    public AudioSource audioSource;

    public Camera mainCamera;
    public GameObject player;
    private PlayerController playerController;

    private Vector3 cameraOffset; // Horizontal and vertical offset for the camera
    public float cameraHeight = 2.75f; // Height above the player

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
            return;
        }

        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController not found on the player GameObject or its children!");
        }

        if (mainCamera != null && player != null)
        {
            // Calculate the offset with the desired height
            cameraOffset = new Vector3(0, cameraHeight, 0);
        }
        else
        {
            Debug.LogError("MainCamera or Player is not assigned!");
        }
    }

    private void Update()
    {
        if (IsPlayerMakingNoise())
        {
            ChasePlayer();
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamageAndKnockback(collision.gameObject);
        }
    }

    private void ApplyDamageAndKnockback(GameObject player)
    {
        Debug.Log($"ApplyDamageAndKnockback called for: {player.name}");

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);
            Debug.Log($"Dealt {damageAmount} damage to player.");
        }
        else
        {
            Debug.LogError("PlayerStats component is missing on the player!");
        }

        // Ensure knockback only applies on horizontal plane
        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
        knockbackDirection.y = 0f; // Remove vertical component to avoid launching the player

        StartCoroutine(ApplyKnockback(player, knockbackDirection, knockbackForce, 0.2f)); // Shorter duration for small knockback

        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
            Debug.Log("Played attack sound.");
        }
        else
        {
            if (audioSource == null)
                Debug.LogError("AudioSource is not assigned to the monster!");
            if (attackSound == null)
                Debug.LogError("Attack sound is not assigned!");
        }
    }

    private IEnumerator ApplyKnockback(GameObject player, Vector3 direction, float force, float duration)
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is null! Ensure it is correctly assigned.");
            yield break;
        }

        // Capture the player's current Y position to maintain during knockback
        float initialYPosition = player.transform.position.y;

        direction.y = 0f; // Ensure no vertical force is applied
        direction.Normalize();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the knockback strength for this frame
            float knockbackStep = force * (1 - (elapsedTime / duration)); // Decelerates over time
            Vector3 movement = direction * knockbackStep; // Apply force directly without extra scaling

            Debug.Log($"knockbackStep: {knockbackStep}");
            Debug.Log($"movement: {movement}");

            // Apply movement and maintain the initial Y position
            Vector3 newPosition = player.transform.position + movement * Time.deltaTime; // Apply movement scaled by delta time
            newPosition.y = initialYPosition; // Lock Y position to ground level

            // Update player's position
            playerController.UpdatePosition(newPosition);

            // Smoothly update the camera position
            if (mainCamera != null)
            {
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    newPosition + cameraOffset,
                    elapsedTime / duration
                );
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Knockback movement completed.");
    }


}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BlindMonsterAI : MonoBehaviour
{
    public float hearingRadius = 10f;
    public NavMeshAgent navMeshAgent;
    public float damageAmount = 20f;
    public float knockbackForce = 70f;
    public LayerMask groundLayer;

    public AudioClip attackSound;
    public AudioSource audioSource;

    public Camera mainCamera;
    public GameObject player;
    private PlayerController playerController;

    private Vector3 cameraOffset;
    public float cameraHeight = 1.75f;

    public float Health = 500f; 
    public float maxHealth = 500f;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
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

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        Debug.Log("Bullet hit new health is : " + Health);

        if (Health <= 0) {
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
        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
        knockbackDirection.y = 0f;

        StartCoroutine(ApplyKnockback(player, knockbackDirection, knockbackForce, 0.2f));

        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
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
        float initialYPosition = player.transform.position.y;

        direction.y = 0f;
        direction.Normalize();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float knockbackStep = force * (1 - (elapsedTime / duration));
            Vector3 movement = direction * knockbackStep * Time.deltaTime;

            Vector3 intendedPosition = player.transform.position + movement;

            if (Physics.Raycast(player.transform.position, movement.normalized, out RaycastHit hit, movement.magnitude, ~0, QueryTriggerInteraction.Ignore))
            {
                intendedPosition = hit.point - movement.normalized * 0.1f;
            }

            intendedPosition.y = initialYPosition;

            playerController.UpdatePosition(intendedPosition);

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



}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class MonsterCombatHandler : MonoBehaviour
{
    public float Health;
    public float maxHealth;

    public float damageAmount;
    public float knockbackForce;
    public float lowHealthThreshold;

    public GameObject player;                      
    private PlayerController playerController;
    private PlayerStats playerStats;

    public PromptUser promptManager;            
    public Transform teleportDestination;         

    public Camera mainCamera;
    public float cameraHeight = 1.75f;            
    private Vector3 cameraOffset;

    private Vector3 initialPosition;
    private Rigidbody rb;

    private Coroutine freezeCoroutine;
    public bool isBlinded = false;

    public GameObject itemDropPrefab;

    public NavMeshAgent navMeshAgent;

    private void Start()
    {
        float difficultyMode = PlayerPrefs.GetFloat("DifficultyMode", 1f);

        Health = Health * difficultyMode;
        maxHealth = maxHealth * difficultyMode;

        damageAmount = damageAmount * difficultyMode;
        knockbackForce = knockbackForce * difficultyMode;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();

            mainCamera = player.GetComponentInChildren<Camera>();

            Transform childTransform = player.transform.Find("Player");

            if (childTransform != null)
            {
                playerStats = childTransform.GetComponent<PlayerStats>();
            }

        }

        initialPosition = transform.position;
        cameraOffset = new Vector3(0, cameraHeight, 0);


        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();

            mainCamera = player.GetComponentInChildren<Camera>();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colided with player");
            ApplyDamageAndKnockback(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(25);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("SniperBullet"))
        {
            TakeDamage(100);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("SMGBullet"))
        {
            TakeDamage(15);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;

        if (Health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    public float GetHealthRatio()
    {
        return Health / maxHealth;
    }

    public bool IsHealthLow()
    {
        return Health <= lowHealthThreshold;
    }

    public bool TakenDMG() 
    {
        return !(Health == maxHealth);
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        Debug.Log("Freezing monster for " + duration + " seconds.");
        isBlinded = true;

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }

        yield return new WaitForSeconds(duration);

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
        isBlinded = false;
        Debug.Log("Unfreezing monster.");
        freezeCoroutine = null; 
    }

    public void Freeze(float duration)
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }
        freezeCoroutine = StartCoroutine(FreezeCoroutine(duration));
    }

    private void ApplyDamageAndKnockback(GameObject playerObj)
    {
        if (playerStats != null)
        {
            Debug.Log("Player stats found");
            playerStats.TakeDamage(damageAmount);
            Debug.Log("Demage found");
            if (playerStats.getHealth() <= 0)
            {
                playerStats.Heal();

                ResetMonsterPosition();

                if (teleportDestination != null)
                {
                    playerController.UpdatePosition(teleportDestination.position);
                }
            }
        }

        Vector3 knockbackDirection = (playerObj.transform.position - transform.position).normalized;
        knockbackDirection.y = 0f;

        StartCoroutine(ApplyKnockback(playerObj, knockbackDirection, knockbackForce, 0.2f));
    }

    private IEnumerator ApplyKnockback(GameObject playerObj, Vector3 direction, float force, float duration)
    {
        float elapsedTime = 0f;
        float initialYPosition = playerObj.transform.position.y;
        direction.Normalize();

        while (elapsedTime < duration)
        {
            float knockbackStep = force * (1 - (elapsedTime / duration));
            Vector3 movement = direction * knockbackStep * Time.deltaTime;
            Vector3 intendedPosition = playerObj.transform.position + movement;

            // Check if the new position is valid
            if (!IsPositionBlocked(playerObj.transform.position, intendedPosition, direction, movement.magnitude))
            {
                // Only update position if it is valid
                playerController.UpdatePosition(intendedPosition);

                // Optionally, update the camera position
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    intendedPosition + cameraOffset,
                    elapsedTime / duration
                );
            }
            else
            {
                // Stop knockback if a collision is detected
                Debug.Log("Collision detected during knockback, stopping.");
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsPositionBlocked(Vector3 currentPosition, Vector3 intendedPosition, Vector3 direction, float distance)
    {
        // Use CapsuleCast for better collision detection
        float capsuleHeight = 2.0f; // Adjust based on player height
        float capsuleRadius = 0.5f; // Adjust based on player size

        Vector3 capsuleBottom = currentPosition + Vector3.up * 0.5f;
        Vector3 capsuleTop = currentPosition + Vector3.up * (capsuleHeight - 0.5f);

        if (Physics.CapsuleCast(
                capsuleBottom,
                capsuleTop,
                capsuleRadius,
                direction,
                out RaycastHit hit,
                distance,
                ~0, // Check against all layers
                QueryTriggerInteraction.Ignore))
        {
            // Log collision details for debugging
            Debug.Log($"Knockback blocked by {hit.collider.name} at distance {hit.distance}");
            return true;
        }

        return false;
    }





    private void ResetMonsterPosition()
    {
        transform.position = initialPosition;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void DropItem()
    {
        Vector3 dropPosition = transform.position + new Vector3(0, 4f, 0);
        Instantiate(itemDropPrefab, dropPosition, Quaternion.identity);
    }

}

using System.Collections;
using UnityEngine;

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

    public PromptUser promptManager;            
    public Transform teleportDestination;         

    public Camera mainCamera;
    public float cameraHeight = 1.75f;            
    private Vector3 cameraOffset;

    private Vector3 initialPosition;
    private Rigidbody rb;

    private void Start()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        initialPosition = transform.position;
        cameraOffset = new Vector3(0, cameraHeight, 0);


        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamageAndKnockback(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(50);  
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;

        if (Health <= 0)
        {
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

    private void ApplyDamageAndKnockback(GameObject playerObj)
    {
        PlayerStats playerStats = playerObj.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);

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

            if (Physics.Raycast(
                playerObj.transform.position,
                movement.normalized,
                out RaycastHit hit,
                movement.magnitude,
                ~0,
                QueryTriggerInteraction.Ignore))
            {
                intendedPosition = hit.point - movement.normalized * 0.1f;
            }

            intendedPosition.y = initialYPosition;


            playerController.UpdatePosition(intendedPosition);
           

            mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    intendedPosition + cameraOffset,
                    elapsedTime / duration
                );

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    private void ResetMonsterPosition()
    {
        transform.position = initialPosition;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}

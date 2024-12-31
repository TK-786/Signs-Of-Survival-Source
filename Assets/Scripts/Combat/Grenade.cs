using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 1f;
    public float explosionRadius = 10f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;

    private Collider grenadeCollider;
    private PickUpScript pickUpScript;

    void Start()
    {
        grenadeCollider = GetComponent<Collider>();
        pickUpScript = Camera.main.GetComponent<PickUpScript>();
    }

    public void Throw()
    {
        // Enable Rigidbody for throwing

        // Ignore collisions with the monster
        IgnoreMonsterCollisions(true);
        pickUpScript.ThrowObject();

        // Start the explosion countdown
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        // Re-enable collisions with monsters
        IgnoreMonsterCollisions(false);

        Explode();
    }

    private void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Detect nearby objects within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Apply damage to monsters
            BlindMonsterAI monster = nearbyObject.GetComponent<BlindMonsterAI>();
            if (monster != null)
            {
                monster.TakeDamage(100); // Adjust damage as needed
                Debug.Log("Monster hit by grenade! Health decreased.");
            }
        }

        Destroy(gameObject); // Destroy the grenade
    }

    private void IgnoreMonsterCollisions(bool ignore)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Monster")) // Ensure monsters are tagged as "Monster"
            {
                Physics.IgnoreCollision(grenadeCollider, collider, ignore);
            }
        }
    }
}

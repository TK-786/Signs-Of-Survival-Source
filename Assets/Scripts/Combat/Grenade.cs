using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{     
    public float explosionDelay = 3f;   
    public float explosionRadius = 5f;  
    public float explosionForce = 700f; 
    public GameObject explosionEffect;

    private PickUpScript pickUpScrip;

    void Start()
    {
        pickUpScrip = Camera.main.GetComponent<PickUpScript>();
    }

    public void Throw()
    {
        pickUpScrip.ThrowObject();
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }

    private void Explode()
    {
        
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            
            if (nearbyObject.CompareTag("Player"))
            {
                Debug.Log("Player hit by grenade!");
            }
        }

        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
    
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

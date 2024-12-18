using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float throwForce = 10f;      
    public float explosionDelay = 3f;   
    public float explosionRadius = 5f;  
    public float explosionForce = 700f; 
    public GameObject explosionEffect;  

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        rb.isKinematic = false; 
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.VelocityChange);
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

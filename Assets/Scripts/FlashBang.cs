using System.Collections;
using UnityEngine;

public class Flashbang : MonoBehaviour
{
    public float throwForce = 500f;     
    public float flashRadius = 10f;      
    public float flashDuration = 3f;     
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        rb.isKinematic = false; 
        rb.AddForce(Camera.main.transform.forward * throwForce); 
        StartCoroutine(TriggerFlash());
    }

    private IEnumerator TriggerFlash()
    {
        yield return new WaitForSeconds(2f); 

        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, flashRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player")) 
            {
                PlayerController player = hit.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.StartCoroutine(player.ApplyFlashEffect(flashDuration));
                }
            }
        }

        Destroy(gameObject); 
    }
}

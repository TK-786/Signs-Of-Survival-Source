using System.Collections;
using UnityEngine;

public class Flashbang : MonoBehaviour
{    
    public float flashRadius = 10f;      
    public float flashDuration = 3f;
    private PickUpScript pickUpScrip;

    void Start()
    {
        pickUpScrip = Camera.main.GetComponent<PickUpScript>();
    }

    public void Throw()
    {
        pickUpScrip.ThrowObject();
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

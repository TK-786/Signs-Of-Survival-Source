using System.Collections;
using UnityEngine;

public class Flashbang : MonoBehaviour, IUsable
{
    public float flashRadius = 10f;     
    public float flashDuration = 3f;

    private PickUpScript pickUpScrip;

    void Start()
    {
        pickUpScrip = Camera.main.GetComponent<PickUpScript>();
    }

    public void OnUse()
    {
        pickUpScrip.ThrowObject();

        StartCoroutine(TriggerFlash());
    }

    private IEnumerator TriggerFlash()
    {
        yield return new WaitForSeconds(1f); // Time before the flashbang explodes

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, flashRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Monster"))
            {
                MonsterCombatHandler monster = hit.GetComponent<MonsterCombatHandler>();
                if (monster != null)
                {
                    Debug.Log($"Flashbang effect duration: {flashDuration}");
                    monster.Freeze(flashDuration);
                    Debug.Log("Monster blinded and frozen by flashbang!");
                }
            }
        }

        Destroy(gameObject); // Destroy the flashbang object
    }



}

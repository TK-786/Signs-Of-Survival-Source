using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UltimatePotion : MonoBehaviour, IUsable
{
    public float raycastDistance = 5f;

    public void OnUse()
    {
        Debug.Log("called");
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance))
        {
            Debug.Log("hit");
            if (hit.collider.gameObject.name == "Vent")
            {
                Debug.Log("UltimatePotion activated");
                Destroy(gameObject);
                DestroyImmediate(gameObject, true);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class FuelReader : MonoBehaviour
{
    public float checkRange = 3f;
    public LayerMask detectionLayers;
    public int count = 0;
    private ArrayList detectedObjects = new ArrayList();

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up * checkRange, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, checkRange, detectionLayers);
        ArrayList currentDetectedObjects = new ArrayList();

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.name == "Fuel")
            {
                currentDetectedObjects.Add(hit.transform);

                if (!detectedObjects.Contains(hit.transform))
                {
                    count++;
                    GameManager.IncrementFuel();
                }
            }
        }

        foreach (Transform previouslyDetected in detectedObjects)
        {
            if (!currentDetectedObjects.Contains(previouslyDetected))
            {

                count--;
                GameManager.DecrementFuel();

            }
        }

        detectedObjects = currentDetectedObjects;
    }
}

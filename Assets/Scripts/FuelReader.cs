using UnityEngine;
using System.Collections;

public class FuelReader : MonoBehaviour
{
    public float checkRange = 3f; // Range for detecting fuel objects
    public LayerMask detectionLayers; // Layers to check during raycasting
    private ArrayList detectedObjects = new ArrayList(); // List of currently detected fuel objects

    void Update()
    {
        // Draw a debug ray upwards for visualization
        Debug.DrawRay(transform.position, Vector3.up * checkRange, Color.red);

        // Perform raycasting to detect objects within range
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, checkRange, detectionLayers);
        ArrayList currentDetectedObjects = new ArrayList();

        // Add newly detected fuel objects to the list
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.name == "Fuel")
            {
                currentDetectedObjects.Add(hit.transform);

                // Increment fuel if this object wasn't detected before
                if (!detectedObjects.Contains(hit.transform))
                {
                    GameManager.IncrementFuel();
                }
            }
        }

        // Remove objects that are no longer detected
        foreach (Transform previouslyDetected in detectedObjects)
        {
            if (!currentDetectedObjects.Contains(previouslyDetected))
            {
                GameManager.DecrementFuel();
            }
        }

        // Update the list of detected objects
        detectedObjects = currentDetectedObjects;
    }
}

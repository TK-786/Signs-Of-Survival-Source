using UnityEngine;

public class EngineReader : MonoBehaviour
{
    public float checkRange = 3f;
    public LayerMask detectionLayers;
    private bool isObjectDetected = false;

    void Update()
    {
        Vector3 rayDirection = transform.forward;

        Debug.DrawRay(transform.position, rayDirection * checkRange, Color.blue);

        RaycastHit hit;
        bool hitDetected = Physics.Raycast(transform.position, rayDirection, out hit, checkRange, detectionLayers);

        if (hitDetected)
        {
            if (!isObjectDetected && hit.transform.name.Contains("Engine"))
            {
                isObjectDetected = true;
                GameManager.RepairedShip();
            }
        }
        else
        {
            if (isObjectDetected)
            {
                isObjectDetected = false;
                GameManager.BreakShip();
            }
        }
    }
}

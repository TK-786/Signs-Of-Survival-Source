using UnityEngine;

public class EngineReader : MonoBehaviour
{
    public float checkRange = 2f;
    public LayerMask detectionLayers;
    private bool isObjectDetected = false;
    private StoryManager storyManager;
    void Start(){
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
    }

    void Update()
    {
        // Set the direction for the raycast
        Vector3 rayDirection = transform.forward;

        // Visualize the ray in the editor
        Debug.DrawRay(transform.position, rayDirection * checkRange, Color.blue);

        // Perform the raycast to detect objects within range
        RaycastHit hit;
        bool hitDetected = Physics.Raycast(transform.position, rayDirection, out hit, checkRange, detectionLayers);

        if (hitDetected)
        {
            // If an "Engine" is detected for the first time, mark as repaired
            if (!isObjectDetected && hit.transform.name.Contains("Engine"))
            {
                isObjectDetected = true;
                GameManager.RepairedShip();
                storyManager.AdvanceStoryEvent(10);
            }
        }
        else
        {
            // If an "Engine" was previously detected but is no longer detected, mark as broken
            if (isObjectDetected)
            {
                isObjectDetected = false;
                GameManager.BreakShip();
            }
        }
    }
}

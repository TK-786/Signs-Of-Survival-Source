using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BunkerInteraction : MonoBehaviour
{
    public Transform bunkerDoor;          // The door's position
    public float interactionDistance = 3f; // Max interaction distance
    public string nextSceneName = "Bunker";  // Name of the scene to load

    private InputAction interactAction;

    void Awake()
    {
        var playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    void OnEnable()
    {
        interactAction.Enable();
        // interactAction.performed += Interact;
    }

    void OnDisable()
    {
        interactAction.Disable();
        // interactAction.performed -= Interact;
    }

    void Update()
    {
        
    }

    public void Interact(InputAction.CallbackContext context)
    {
        LoadNextScene();
    }
    public void LoadNextScene()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.SphereCast(ray, 0.1f, out hit, interactionDistance))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name} with tag: {hit.collider.tag}");

            if (hit.collider.CompareTag("BunkerDoor"))
            {
                string currentScene = SceneManager.GetActiveScene().name;

                // Determine the next scene based on current scene
                string nextSceneName = (currentScene == "Outside") ? "Bunker" : "Outside";

                Debug.Log($"Transitioning from {currentScene} to {nextSceneName}");

                // Load the next scene
                GameManager.instance.LoadNextScene(nextSceneName);

            }
        }
    }
}
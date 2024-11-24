using UnityEngine;

public class TeleportationToShip : MonoBehaviour
{
    public Transform teleportDestination; // Destination point where the player will be teleported
    public GameObject player; // The player game object
    public Camera mainCamera; // The main camera in the scene

    private PlayerController playerController; // Reference to the PlayerController component

    private void Start()
    {
        // Gets the PlayerController component from the player or its children
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If a collider with the "Player" tag enters the trigger, teleport the player
        if (other.CompareTag("Player"))
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        // Teleports the player to the specified destination and updates the camera position
        if (playerController != null)
        {
            playerController.UpdatePosition(teleportDestination.position);
            //mainCamera.transform.position = teleportDestination.position;
        }
    }
}

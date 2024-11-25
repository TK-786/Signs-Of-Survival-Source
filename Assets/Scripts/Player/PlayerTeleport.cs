using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportDestination; // Destination point for teleportation
    public GameObject player; // Player game object
    public Camera mainCamera; // Main camera in the scene

    private PlayerController playerController; // Reference to the PlayerController component

    private void Start()
    {
        // Get the PlayerController component from the player or its children
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Teleport the player if they enter the trigger and have the "Player" tag
        if (other.CompareTag("Player"))
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        // Teleports the player and updates the camera position if the PlayerController is available
        if (playerController != null)
        {
            playerController.UpdatePosition(teleportDestination.position);
            //mainCamera.transform.position = teleportDestination.position;
        }
    }
}

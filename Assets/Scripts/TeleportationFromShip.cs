using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationFromShip : MonoBehaviour
{
    public Transform teleportDestination;
    public GameObject player;
    public Camera mainCamera;

    private PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        if (playerController != null)
        {
            playerController.UpdatePosition(teleportDestination.position);

            mainCamera.transform.position = teleportDestination.position;
        }
    }
}

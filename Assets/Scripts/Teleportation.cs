using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Player"))
        {

            Debug.Log("Player has entered the teleport zone.");
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        Debug.Log("Teleporting player to: " + teleportDestination.position);
        player.position = teleportDestination.position;
    }

}

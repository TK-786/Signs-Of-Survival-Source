using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    private PlayerStats playerStats; // Reference to PlayerStats component
    private PlayerController playerController;
    private Item item;

    void Start()
    {
        item = GetComponent<Item>();

        // Find the player GameObject by tag
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerStats = playerController.playerStats;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Make sure the Player is tagged as 'Player'.");
        }
    }

    void Update()
    {
        if (item != null && item.isHeld && Input.GetMouseButtonDown(0))
        {
            if (playerController != null)
            {
                playerStats.Heal();
                Debug.Log("Player healed!");
            }
            else
            {
                Debug.LogError("Cannot heal: playerController reference is missing!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingPotion : MonoBehaviour, IUsable
{
    private PlayerController playerController;
    private Item item;

    private InputAction useAction;

    void Start()
    {
        item = GetComponent<Item>();

        GameObject player = GameObject.FindWithTag("Player");

        playerController = player.GetComponent<PlayerController>();

    }

    public void OnUse()
    {
        if (item != null && item.isHeld)
        {
            playerController.playerStats.Heal();

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AdrenalinePotion : MonoBehaviour, IUsable
{
    private PlayerController playerController;
    private Item item;

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
            playerController.StartCoroutine(playerController.BoostPlayerStats());
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthPotion : MonoBehaviour
{
    private PlayerController playerController;
    private Item item;

    void Start()
    {
        item = GetComponent<Item>();

        GameObject player = GameObject.FindWithTag("Player");

        playerController = player.GetComponent<PlayerController>();

    }

    void Update()
    {
        if (item != null && item.isHeld && Input.GetMouseButtonDown(0))
        {
            playerController.StartCoroutine(playerController.SilencePlayer());

            Destroy(gameObject);
        }
    }
}

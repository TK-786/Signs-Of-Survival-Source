using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdranalinePotion : MonoBehaviour
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
        Debug.Log("Item is being held: " + item.isHeld);
        if (item != null && item.isHeld && Input.GetMouseButtonDown(0))
        {
            playerController.StartCoroutine(playerController.BoostPlayerStats());

            Destroy(gameObject);
        }
    }
}

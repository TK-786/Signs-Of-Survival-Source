using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AdrenalinePotion : MonoBehaviour
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

    private void OnDisable()
    {
        if (useAction != null)
        {
            useAction.performed -= OnUse;
            useAction.Disable();
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (item != null && item.isHeld)
        {
            playerController.StartCoroutine(playerController.BoostPlayerStats());
            Destroy(gameObject);
        }
    }
}

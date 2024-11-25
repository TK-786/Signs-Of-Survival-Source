using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject Menu;
    private bool menuOpen;
    public ItemSlot[] itemSlots;
    private PickUpScript pickUpScript;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        Menu.SetActive(false);
        pickUpScript = Camera.main.GetComponent<PickUpScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")){
            Menu.SetActive(!menuOpen);  
            menuOpen = !menuOpen;
            Cursor.visible = menuOpen;
            if (menuOpen){
                Cursor.lockState = CursorLockMode.None;
            } else{
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void AddItem(string name, int quantity, Sprite icon, string itemDescription, int stackLimit, Item item){
        if (item == null)
        {
            Debug.LogError("Cannot add item: item is null.");
            return;
        }
        
        Debug.Log($"Attempting to add {quantity} of {name} to inventory.");

        if (!CanAddItem(name, quantity, stackLimit))
        {
            Debug.LogWarning($"Not enough space to add {quantity} of {name} to inventory.");
            return;
        }
        
        int remainingQuantity = quantity;

        // stacking in existing slots with the same item type
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].hasItem && itemSlots[i].itemName == name && itemSlots[i].quantity < stackLimit)
            {
                int stackableAmount = Mathf.Min(stackLimit - itemSlots[i].quantity, remainingQuantity);
                itemSlots[i].AddItem(name, stackableAmount, icon, itemDescription, item);
                remainingQuantity -= stackableAmount;

                Debug.Log($"Stacked {stackableAmount} of {name} in slot {i}. Remaining quantity: {remainingQuantity}");

                if (remainingQuantity <= 0) return;
            }
        }

        // try adding to empty slots if theres left over
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].hasItem)
            {
                int addedAmount = Mathf.Min(stackLimit, remainingQuantity);
                itemSlots[i].AddItem(name, addedAmount, icon, itemDescription, item);
                remainingQuantity -= addedAmount;

                Debug.Log($"Added {addedAmount} of {name} to empty slot {i}. Remaining quantity: {remainingQuantity}");

                if (remainingQuantity <= 0) return;
            }
        }

        // place remaining items in player's hands if inventory is full
        if (remainingQuantity > 0)
        {
            Debug.LogWarning($"Inventory full. Placing remaining {remainingQuantity} of {name} in player's hands.");
            item.SetItemData(name, remainingQuantity, icon, itemDescription, item);
            pickUpScript.PickUpObject(item.gameObject);
        }
    }

    public void EquipItem(ItemSlot itemSlot)
    {
        if (itemSlot == null || !itemSlot.hasItem)
        {
            Debug.LogError("ItemSlot is null or not full.");
            return; 
        }
        if (itemSlot.item == null)
        {
            Debug.LogError("Item is null, cannot equip item.");
            return;
        }
        if (itemSlot.hasItem)
        {
            Item equippedItem = itemSlot.item;
            equippedItem.gameObject.SetActive(true);
            Debug.Log("Equipping item: " + equippedItem.name + ", Layer set to holdLayer");
            pickUpScript.PickUpObject(equippedItem.gameObject);
        }
    }

    public bool ContainsItem(string itemName)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.hasItem && slot.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAddItem(string itemName, int quantity, int stackLimit)
    {
        int remainingQuantity = quantity;

        // Check existing stacks for available space
        foreach (var slot in itemSlots)
        {
            if (slot.hasItem && slot.itemName == itemName && slot.quantity < stackLimit)
            {
                int availableSpace = stackLimit - slot.quantity;
                remainingQuantity -= availableSpace;

                if (remainingQuantity <= 0)
                    return true; 
            }
        }

        foreach (var slot in itemSlots)
        {
            if (!slot.hasItem)
            {
                remainingQuantity -= stackLimit;

                if (remainingQuantity <= 0)
                    return true;
            }
        }

        return false;
    }
}
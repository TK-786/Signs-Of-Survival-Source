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

    public void AddItem(string name, int quantity, Sprite icon, string itemDescription, Item item){
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(!itemSlots[i].isFull){
                itemSlots[i].AddItem(name, quantity, icon, itemDescription, item);
                return;
            }
        }
    }

    public void EquipItem(ItemSlot itemSlot)
    {
        if (itemSlot == null || !itemSlot.isFull)
        {
            Debug.LogError("ItemSlot is null or not full.");
            return; 
        }
        if (itemSlot.item == null)
        {
            Debug.LogError("Item is null, cannot equip item.");
            return;
        }
        if (itemSlot.isFull)
        {
            Item equippedItem = itemSlot.item;
            equippedItem.gameObject.SetActive(true);
            Debug.Log("Equipping item: " + equippedItem.name + ", Layer set to holdLayer");
            pickUpScript.PickUpObject(equippedItem.gameObject);
        }
    }
}
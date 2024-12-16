using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite icon;
    public bool hasItem;
    public string itemDescription;
    public Item item;
    
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemIcon;

    public GameObject selectedSlot;
    public bool itemSelected;

    // item desciption components
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDescImage;

    private InventoryManager inventoryManager;

    public ItemSlot[] craftSlots;

    ItemSlot craftedItemSlot; 

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        craftSlots = inventoryManager.GetCraftingSlots();
    }

    public void AddItem(string itemName, int quantity, Sprite icon, string itemDescription, Item item){
        if (!hasItem)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.itemDescription = itemDescription;
            this.item = item;
            hasItem = true;
        }

        this.quantity += quantity;

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        itemIcon.sprite = icon;
        itemIcon.gameObject.SetActive(true); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right){
            OnRightClick();
        }
    }

    public void OnLeftClick(){
        for (int i = 0; i < inventoryManager.itemSlots.Length; i++)
        {
            inventoryManager.itemSlots[i].selectedSlot.SetActive(false);
            inventoryManager.itemSlots[i].itemSelected = false;
        }

        Debug.Log("Left Click detected!");
        if (!inventoryManager.GetMode())
        {
            selectedSlot.SetActive(true);
            itemSelected = true;
            itemNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescImage.sprite = icon;
        }

        if (inventoryManager.GetMode() && hasItem)
        {
            if (System.Array.Exists(craftSlots, slot => slot == this))
            {
                inventoryManager.AddItem(itemName, quantity, icon, itemDescription, item.stackLimit, item);
                ResetItemSlot();
            }
            else if(this == inventoryManager.craftingManager.craftedItemSlot)
            {
                inventoryManager.AddItem(itemName, quantity, icon, itemDescription, item.stackLimit, item);
                ResetItemSlot();
                foreach (ItemSlot slot in craftSlots)
                {
                    slot.ResetItemSlot();
                }
            }
            else
            {
                foreach (ItemSlot slot in craftSlots)
                {
                    if (!slot.hasItem)
                    {
                        slot.AddItem(itemName, quantity, icon, itemDescription, item);
                        ResetItemSlot();
                        break;
                    }
                }
            }
            inventoryManager.craftingManager.PreviewCraftItem();
        }

    }

    public void OnRightClick(){
        if (quantity < 1)
        {
            Debug.Log("No items available to equip.");
            return; 
        }
        Debug.Log("Right Click detected! Equipping item: " + itemName);

        inventoryManager.EquipItem(this);
        ResetItemSlot();

        // quantity--; 
        // if (quantity <= 0)
        // {
        //     ResetItemSlot(); // Reset the slot if no items are left
        // }
    }

    public void ResetItemSlot() {
        // Reset the slot properties
        itemName = "";                      
        quantity = 0;                       
        icon = null;                        
        hasItem = false;                     
        itemDescription = "";               
        item = null;                        

        // Hide the quantity text and item icon
        quantityText.text = "";             
        quantityText.enabled = false;       
        itemIcon.sprite = null;             
        itemIcon.gameObject.SetActive(false); 

        itemNameText.text = "";             
        itemDescriptionText.text = "";       
        itemDescImage.sprite = null;         

    }
}

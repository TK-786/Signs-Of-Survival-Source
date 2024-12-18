using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite icon;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    [SerializeField]
    private int stackLimit;

    public InventoryManager inventoryManager;
    public Item item;
    public Boolean isHeld;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
   
    public void AddToInventory(){
        inventoryManager.AddItem(itemName, quantity, icon, itemDescription, stackLimit, this);
        gameObject.SetActive(false);
    }

    public void SetItemData(string name, int quant, Sprite x, string description, Item item){
        this.itemName = name;
        this.quantity = quant;
        this.icon = x;
        this.itemDescription = description;
        this.item = item;
    }

    // Read-only properties
    public string ItemName => itemName;           
    public int Quantity => quantity;               
    public Sprite Icon => icon;                    
    public string ItemDescription => itemDescription; 
    public Item ItemObj => item;    
    public int StackLimit => stackLimit;
}
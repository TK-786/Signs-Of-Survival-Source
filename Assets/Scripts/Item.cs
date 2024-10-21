using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite icon;

    [TextArea]
    [SerializeField]
    private String itemDescription;

    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }


    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            inventoryManager.addItem(itemName, quantity, icon, itemDescription);
            Destroy(gameObject);
        }
    }
    public void setItemData(string name, int quant, Sprite x){
        this.itemName = name;
        this.quantity = quant;
        this.icon = x;
    }
}


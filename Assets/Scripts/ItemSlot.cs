using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Sprite icon;
    public bool isFull;

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemIcon;

    public void addItem(string itemName, int quantity, Sprite icon){
        this.itemName = itemName;
        this.quantity = quantity;
        this.icon = icon;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemIcon.sprite = icon;
        itemIcon.gameObject.SetActive(true); 
        isFull = true;
    }    
}

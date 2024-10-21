using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite icon;
    public bool isFull;

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemIcon;

    public GameObject selectedSlot;
    public bool itemSelected;

    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right){
            OnRightClick();
        }
    }

    public void OnLeftClick(){
        for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
        {
            inventoryManager.itemSlot[i].selectedSlot.SetActive(false);
        }

        Debug.Log("Left Click detected!");
        selectedSlot.SetActive(true);
        itemSelected = true;
    }

    public void OnRightClick(){
        if (quantity < 1){return;}
        GameObject itemToDrop = new GameObject(itemName);
        
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.setItemData(itemName, quantity, icon);
        
        Rigidbody rb = itemToDrop.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        SphereCollider collider = itemToDrop.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;  

        GameObject skin = new GameObject("skin");
        skin.transform.parent = itemToDrop.transform;

        MeshFilter mf = skin.AddComponent<MeshFilter>();
        MeshRenderer mr = skin.AddComponent<MeshRenderer>();
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tempSphere.transform.position = new Vector3(0, -1000, 0);  
    
        mf.mesh = tempSphere.GetComponent<MeshFilter>().sharedMesh;
        
        GameObject.Destroy(tempSphere);
        
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        Vector3 forwardDirection = player.transform.forward;
        
        Vector3 dropPosition = playerPosition + forwardDirection * 1f;
        itemToDrop.transform.position = dropPosition;
        itemToDrop.transform.localScale = new Vector3(1f, 1f, 1f);  
        
        ResetItemSlot();
        if (itemSelected){
            selectedSlot.SetActive(false);
            itemSelected = false;
        }

        
    }
    private void ResetItemSlot() {
        // Reset the slot properties
        itemName = "";
        quantity = 0;
        icon = null;
        isFull = false;

        // Hide the quantity text and item icon
        quantityText.text = "";
        quantityText.enabled = false;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }
}

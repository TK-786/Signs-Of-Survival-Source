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
        Debug.Log("Left Click detected!");
        selectedSlot.SetActive(true);
        itemSelected = true;
    }

    public void OnRightClick(){
        GameObject itemToDrop = new GameObject(itemName);
        
        // Add the item component to store item data
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.setItemData(itemName, quantity, icon);


        // Add a Rigidbody and SphereCollider to the main item object for physics
        Rigidbody rb = itemToDrop.AddComponent<Rigidbody>();
        SphereCollider collider = itemToDrop.AddComponent<SphereCollider>();
        collider.radius = 0.5f;  // Adjust collider size as needed

        // Create and assign the child object that handles the visual mesh (skin)
        GameObject skin = new GameObject("skin");
        skin.transform.parent = itemToDrop.transform;

        // Add MeshFilter and MeshRenderer to the skin for rendering the 3D object
        MeshFilter mf = skin.AddComponent<MeshFilter>();
        MeshRenderer mr = skin.AddComponent<MeshRenderer>();
        GameObject tempSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tempSphere.transform.position = new Vector3(0, -1000, 0);  // Place it under the map
    
        // Assign the mesh from the temporary object to the MeshFilter
        mf.mesh = tempSphere.GetComponent<MeshFilter>().sharedMesh;

        // Destroy the temporary object after extracting the mesh
        GameObject.Destroy(tempSphere);

        // Set the item's location in front of the player
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        Vector3 dropPosition = playerPosition + new Vector3(0, 0, 1f);  // Adjust for how far you want the item to drop in front of the player
        itemToDrop.transform.position = dropPosition;

        // Set the scale of the item (parent object)s
        itemToDrop.transform.localScale = new Vector3(1f, 1f, 1f);  // Adjust the scale as needed
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

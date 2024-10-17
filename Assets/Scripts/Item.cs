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

    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Player"){
            inventoryManager.addItem(itemName, quantity, icon);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string name;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite icon;

    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<inventoryManager>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision3D collision){
        if (collision.gameObject.tag == "player"){
            inventoryManager.addItem(name, quantity, icon);
            Destroy(gameObject);
        }
    }
}

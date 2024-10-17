using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject Menu;
    private bool menuOpen;
    public ItemSlot[] itemSlot;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")){
            Menu.SetActive(!menuOpen);
            menuOpen = !menuOpen;
        }
    }

    public void addItem(string name, int quantity, Sprite icon){
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(!itemSlot[i].isFull){
                itemSlot[i].addItem(name, quantity, icon);
                return;
            }
        }
    }
}

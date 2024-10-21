using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//https://github.com/JonDevTutorial/PickUpTutorial/blob/main/PickUpScript.cs

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;

    //force at which the object is thrown at
    public float throwForce = 500f;

    //how far the player can pickup the object from 
    public float pickUpRange = 5f; 
    private GameObject heldObj; 
    private Rigidbody heldObjRb; 
    private int LayerNumber; 

     
    public GameObject interactionCanvas;
    public TMP_Text interactionText;

      void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); 
        interactionCanvas.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null) 
            {
                // Perform raycast to check if the player is looking at an object within pickUpRange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject(); 
            interactionCanvas.SetActive(true); 
            interactionText.text = "Press F to store in inventory"; 

            if (Input.GetKeyDown(KeyCode.Mouse0)) // Left click to throw
            {
                ThrowObject();
            }

            if (Input.GetKeyDown(KeyCode.F)) 
            {
                Debug.Log("Pressed F to add to inventory.");

                Item item = heldObj.GetComponent<Item>();
                if (item != null)
                {
                    Debug.Log("Item found: " + item.ItemName); 
                }
                else
                {
                    Debug.LogError("Item component is null.");
                }
                if (item != null) 
                {
                    item.AddToInventory(); 
                    ClearHeldObject(); 
                }
            }
        }
        else
        {
            interactionCanvas.SetActive(false); 
        }
    }
    public void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {
            heldObj = pickUpObj; 
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); 
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; 
            heldObj.layer = LayerNumber; 
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }

    // Keep object position at holdPos
    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }
    
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void ClearHeldObject()
    {
        heldObj = null;
        heldObjRb = null;
        interactionCanvas.SetActive(false);  // Hide interaction UI
    }
}

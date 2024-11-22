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
    private int defaultLayer;        
    private int holdLayer; 

     
    public GameObject interactionCanvas;
    public TMP_Text interactionText;

      void Start()
    {
        holdLayer = LayerMask.NameToLayer("holdLayer");
        defaultLayer = LayerMask.NameToLayer("Default");
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
                StopClipping();
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject(); 
            interactionCanvas.SetActive(true); 
            interactionText.text = "Press F to store in inventory"; 

            if (Input.GetKeyDown(KeyCode.Mouse1)) // Right click to throw
            {
                StopClipping();
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
            heldObj.layer = holdLayer; 
            SetLayerRecursive(heldObj, holdLayer);
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = defaultLayer;
        heldObjRb.isKinematic = false;
        Debug.Log("Dropping object: " + heldObj.name + ", Layer set to: " + defaultLayer);
        SetLayerRecursive(heldObj, defaultLayer);
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
        heldObj.layer = defaultLayer;
        heldObjRb.isKinematic = false;
        SetLayerRecursive(heldObj, defaultLayer); 
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

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer; // Set the layer of the object itself
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer); // Recursively set layer for all children
        }
    }

}

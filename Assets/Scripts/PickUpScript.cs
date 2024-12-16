using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform rightHandPos;

    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private int defaultLayer;
    private int holdLayer;
    private bool isEquipped = false;

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
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                StopClipping();
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            interactionCanvas.SetActive(true);
            interactionText.text = "Press F to store in inventory";

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isEquipped)
            {
                EquipObject();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (heldObj != null)
                {
                    Flashbang flashbang = heldObj.GetComponent<Flashbang>();
                    if (flashbang != null)
                    {
                        flashbang.Throw(); 
                        heldObj = null;    
                        isEquipped = false;
                    }
                    else
                    {
                        StopClipping();
                        ThrowObject(); 
                    }
                }
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
            heldObj.transform.parent = holdPos.transform;
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.identity;
            heldObj.layer = holdLayer;

            MeshCollider meshCollider = heldObj.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.isTrigger = true;
            }

            SetLayerRecursive(heldObj, holdLayer);
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = defaultLayer;
        heldObjRb.isKinematic = false;
        SetLayerRecursive(heldObj, defaultLayer);
        heldObj.transform.parent = null;

        MeshCollider meshCollider = heldObj.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.isTrigger = true;
        }

        Item item = heldObj.GetComponent<Item>();
        if (item != null)
        {
            item.isHeld = false;
        }

        heldObj = null;
        isEquipped = false;
    }

    void MoveObject()
    {
        if (!isEquipped)
            {heldObj.transform.position = holdPos.transform.position;}
    }

    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = defaultLayer;
        heldObjRb.isKinematic = false;
        SetLayerRecursive(heldObj, defaultLayer);
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);

        MeshCollider meshCollider = heldObj.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.isTrigger = false;
        }

        Item item = heldObj.GetComponent<Item>();
        if (item != null)
        {
            item.isHeld = false;
        }

        heldObj = null;
        isEquipped = false;
    }

    void EquipObject()
    {
        if (heldObj != null)
        {
            heldObj.transform.SetParent(rightHandPos); 
            heldObj.transform.localPosition = Vector3.zero; 
            heldObj.transform.localRotation = Quaternion.identity;
            heldObj.transform.localRotation *= Quaternion.Euler(0, 90, 0); 
            
            Item item = heldObj.GetComponent<Item>();
            if (item != null)
            {
                item.isHeld = true;
            }

            isEquipped = true;
            Debug.Log("Equipped the object to the right hand.");
        }
    }

    void ClearHeldObject()
    {
        heldObj = null;
        heldObjRb = null;
        interactionCanvas.SetActive(false);
        isEquipped = false;
    }

    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
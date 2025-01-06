using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform rightHandPos;

    public float throwForce = 500f;
    public float pickUpRange = 5f;
    public GameObject heldObj;
    private Rigidbody heldObjRb;
    private int defaultLayer;
    private int holdLayer;
    private bool isEquipped = false;

    void Start()
    {
        holdLayer = LayerMask.NameToLayer("holdLayer");
        defaultLayer = LayerMask.NameToLayer("Default");
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.CompareTag("canPickUp") ||  hit.transform.CompareTag("Weapon"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (heldObj != null)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    public void Use(InputAction.CallbackContext context)
    {

            Debug.Log("Use");
            if (heldObj != null)
            {
                Debug.Log("is held");
                IUsable usable = heldObj.GetComponent<IUsable>();
                if (usable != null) {
                    Debug.Log("usabe");
                    usable.OnUse();
                }
            }
        
    }


    public void Equip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (heldObj != null && !isEquipped)
            {
                EquipObject();
            }
        }
    }

    public void StoreInInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (heldObj != null)
            {
                Item item = heldObj.GetComponent<Item>();
                if (item != null)
                {
                    item.AddToInventory();
                    ClearHeldObject();
                }
            }
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
    public void dropHeldObj(){
        if (heldObj != null){
            StopClipping();
            Item item = heldObj.GetComponent<Item>();
            if (item != null){
                item.isHeld = false;
            }

            heldObj = null;
            isEquipped = false;
        }
    }

    public void DropObject(InputAction.CallbackContext context)
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = defaultLayer;
        heldObjRb.isKinematic = false;
        SetLayerRecursive(heldObj, defaultLayer);
        heldObj.transform.parent = null;

        MeshCollider meshCollider = heldObj.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.isTrigger = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(heldObj.transform.position, Vector3.down, out hit, 1f))
        {
            // Place the item slightly above the floor
            heldObj.transform.position = hit.point + Vector3.up * 0.4f;
        }
        else
        {
            // If no floor detected, slightly raise the item to prevent clipping
            heldObj.transform.position += Vector3.up * 0.2f;
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

    public void ThrowObject()
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
            if (heldObj.name == "sniperNew" || heldObj.name == "AMG")
            {
                heldObj.transform.localRotation *= Quaternion.Euler(270, 90, 0f);
            }
            else {
                heldObj.transform.localRotation *= Quaternion.Euler(0, 90, 0);
            }

            MeshCollider meshCollider = heldObj.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.isTrigger = true;
            }

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
    public GameObject getHeldObj => heldObj;
}
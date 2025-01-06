using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    public TMP_Text myText;
    public Image myImage;
    public Camera mainCamera;
    private PickUpScript pickUpScript;

    void Start()
    {
        pickUpScript = mainCamera.GetComponent<PickUpScript>();
    }

    void Update() {
        checkWeaponHold();
        updateBulletCount();
    }

    void checkWeaponHold()
    {
        if (pickUpScript.heldObj == null || !pickUpScript.heldObj.CompareTag("Weapon"))
        {
            myImage.gameObject.SetActive(false);
            myText.gameObject.SetActive(false);
        }
        else
        {
            myImage.gameObject.SetActive(true);
            myText.gameObject.SetActive(true);
        }
    }

    void updateBulletCount() {
        if (!(pickUpScript.heldObj == null || !pickUpScript.heldObj.CompareTag("Weapon"))) {
            IWeapon weapon = pickUpScript.heldObj.GetComponent<IWeapon>();
            if (weapon != null)
            {
                myText.text = "Ammo: " + weapon.GetCurrentAmmo();
            }
            else
            {
                Debug.LogWarning("Held object does not implement IWeapon.");
                myText.text = "No Weapon";
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class FuelReader : MonoBehaviour
{
    private int fuelAmount = 0; // Number of fuel objects detected
    void Start(){
    
    }
    
    public void depositFuel(){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f)){
            if (hit.collider.gameObject == gameObject){
                Debug.Log("fuel container detected");
                if (fuelAmount >= 10){
                    Dialogue.instance.InitDialogue(new string[]{"I have deposited enough fuel to power the ship!"});
                } else {
                    GameObject obj = Camera.main.gameObject.GetComponent<PickUpScript>().getHeldObj;
                    if(obj.GetComponent<Item>().ItemName == "Fuel Rod"){
                        Destroy(obj);
                        fuelAmount++;
                        GameManager.IncrementFuel();
                    } else {
                        Dialogue.instance.InitDialogue(new string[]{"I need to put in a fuel rod!"});
                    }
                }
            }
        }
    }
    public int getFuelAmount(){
        return fuelAmount;
    }
}

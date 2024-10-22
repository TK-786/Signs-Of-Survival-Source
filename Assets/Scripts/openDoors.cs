using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoors : MonoBehaviour
{
    public Animator leftDoorSlide;
    public Animator rightDoorSlide;
    public float doorCloseDelay = 1.0f;  
    public float cooldownTime = 10.0f;  
    private bool hasBeenOpened = false;  

    private bool isOpen = false;  
    private bool isInCooldown = false;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenOpened && !isInCooldown)
        {
          
            leftDoorSlide.SetTrigger("Open");
            rightDoorSlide.SetTrigger("Open");
            isOpen = true; 
            hasBeenOpened = true; 
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
          
            Invoke("CloseDoors", doorCloseDelay);  
        }
    }

    private void CloseDoors()
    {
        // Trigger the door close animations
        leftDoorSlide.SetTrigger("Close");
        rightDoorSlide.SetTrigger("Close");
        isOpen = false;  // Mark doors as closed

        // Start the cooldown after closing the doors
        StartCoroutine(DoorCooldown());
    }

    private IEnumerator DoorCooldown()
    {
        isInCooldown = true;  // Set the trigger to cooldown mode
        yield return new WaitForSeconds(cooldownTime);  // Wait for the cooldown period to finish
        isInCooldown = false;  // Reset cooldown mode
    }
}

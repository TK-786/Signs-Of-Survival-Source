using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<string> noteText = new List<string>();
    private InputAction interactAction;

    void Start()
    {
        PlayerInput playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();
        interactAction.performed += ReadNote;
    }

    public void ReadNote(InputAction.CallbackContext context){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 6f)){
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("Note detected");
                DisplayNote();
            }
        }
    }

    public void DisplayNote(){
        Debug.Log("Displaying note");
        Dialogue.instance.InitDialogue(noteText.ToArray());
    }    
}

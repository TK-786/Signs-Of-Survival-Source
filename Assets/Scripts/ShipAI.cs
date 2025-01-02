using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject uiPanel;
    [SerializeField] List<Button> options = new();
    bool isActive = false;
    private InputAction InteractShipConsole;
    BehaviorTree rootNode = null;
    void Start()
    {
        PlayerInput playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        InteractShipConsole = playerInput.actions["CraftingMode"];
        InteractShipConsole.Enable();
        InteractShipConsole.performed += displayConsoleOptions;

        rootNode = new BehaviorTree();
        SequenceNode interactOptionsSequence = new SequenceNode();
        interactOptionsSequence.AddChild(new ActionNode(new DisplayOptionsAction(uiPanel, "Hello, How can i help?", new String[] {"Whats the time?", "Whats the weather?", "What should i do next?"}, options)));
        interactOptionsSequence.AddChild(new ActionNode(new WaitForOptionAction(uiPanel, options)));
        rootNode.AddChild(interactOptionsSequence);
    }

    public void displayConsoleOptions(InputAction.CallbackContext context){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f)){
            if (hit.collider.gameObject.name == "shipConsole"){
                Debug.Log("Ship console detected");
                isActive = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && rootNode != null){
            Debug.Log("Displaying options");
            NodeStatus status = rootNode.Execute();
            if (status == NodeStatus.Success){
                isActive = false;
            }
        }
    }
}

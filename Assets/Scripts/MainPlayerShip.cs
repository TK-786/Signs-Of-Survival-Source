using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainPlayerShip : MonoBehaviour
{
    // Start is called before the first frame update
    private InputAction interactAction;
    EngineReader engineReader;
    FuelReader fuelReader;
    ShipAI shipAI;

    void Start()
    {
        PlayerInput playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];

        engineReader = GameObject.Find("EngineReader").GetComponent<EngineReader>();
        fuelReader = GameObject.Find("fuelRodContainer").GetComponent<FuelReader>();
        shipAI = GameObject.Find("shipConsole").GetComponent<ShipAI>();

        interactAction.Enable();
        interactAction.performed += InsertEngine;
        interactAction.performed += DepositFuel;
        interactAction.performed += DisplayConsoleOptions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertEngine(InputAction.CallbackContext context){
        engineReader.InsertEngine();
    }

    public void DepositFuel(InputAction.CallbackContext context){
        fuelReader.depositFuel();
    }

    public void DisplayConsoleOptions(InputAction.CallbackContext context){
        shipAI.DisplayConsoleOptions();
    }
}

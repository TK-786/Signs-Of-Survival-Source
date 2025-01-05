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
    private bool isActive = false;
    private bool isUIOpen = false;
    private InputAction InteractShipConsole;
    void Start()
    {
        PlayerInput playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        InteractShipConsole = playerInput.actions["CraftingMode"];
        InteractShipConsole.Enable();
        InteractShipConsole.performed += displayConsoleOptions;
    }

    public void displayConsoleOptions(InputAction.CallbackContext context){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f)){
            if (hit.collider.gameObject.name == "shipConsole"){
                Debug.Log("Ship console detected");
                isActive = true;
                ShowOptionsUI();
            }
        }
    }

   private void ShowOptionsUI()
    {
        if (isUIOpen) return;

        isUIOpen = true;
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Assign Button Actions
        AssignButtonActions();
    }

    private void AssignButtonActions()
    {
        // Clear any existing listeners to avoid stacking issues
        foreach (Button button in options)
        {
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
        }

        // Assign actions for each button
        options[0].onClick.AddListener(() => DisplayTime());
        options[1].onClick.AddListener(() => DisplayWeather());
        options[2].onClick.AddListener(() => DisplayHints());
        options[3].onClick.AddListener(() => CloseUI()); // Assuming a close button
    }

    private void DisplayTime()
    {
        TickManager tickManager = TickManager.Instance;
        
        Debug.Log("Current Time: " + tickManager.Hours + ":" + tickManager.Minutes);
        CloseUI();
    }

    private void DisplayWeather()
    {
        Debug.Log("Weather: Clear skies");
        CloseUI();
    }

    private void DisplayHints()
    {
        Debug.Log("Hint: Follow the beacon!");
        CloseUI();
    }

    private void CloseUI()
    {
        isActive = false;
        isUIOpen = false;
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach(Button button in options){
            button.gameObject.SetActive(false);
        }
    }

    // Debugging purposes, you can remove this later
    void Update()
    {
        if (isActive && !isUIOpen)
        {
            Debug.Log("Ship Console Interaction Ready. Press the Interaction Key.");
        }
    }
}

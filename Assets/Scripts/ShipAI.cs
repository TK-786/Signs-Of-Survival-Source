using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipAI : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject uiPanel;
    List<Button> options = new();
    private bool isActive = false;
    private bool isUIOpen = false;
    List<string> hints = new List<string>();
    CutsceneManager cutsceneManager;
        
    void Start()
    {
        uiPanel = GameObject.Find("ButtonBox");
        options.Add(uiPanel.transform.Find("Button1").GetComponent<Button>());
        options.Add(uiPanel.transform.Find("Button2").GetComponent<Button>());
        options.Add(uiPanel.transform.Find("Button3").GetComponent<Button>());
        options.Add(uiPanel.transform.Find("Button4").GetComponent<Button>());
        options.Add(uiPanel.transform.Find("Button5").GetComponent<Button>());
        options.Add(uiPanel.transform.Find("Button6").GetComponent<Button>());

        uiPanel.SetActive(false);

        hints.Add("I detected another crashed ship in the area as we came down, it could be worth investigating");
        hints.Add("There seem to be hostile life forms in the area, might serve you well to arm yourself");
        hints.Add("My radars are picking up useful plants and items around, you could try to craft something with them");
        hints.Add("The bunker should have an engine as a backup generator, perhaps it could be repurposed");
        hints.Add("The scientists who used to inhabit the bunker must have had some useful documents, they could be worth a look");

        cutsceneManager = GameObject.Find("CrashCutscene").GetComponent<CutsceneManager>();
    }

    public void DisplayConsoleOptions(){
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f)){
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
        List<string> dialogue = new List<string>();
        dialogue.Add("How can I help you?");
        Dialogue.instance.InitDialogue(dialogue.ToArray());
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
        options[3].onClick.AddListener(() => DisplayRepair()); 
        options[4].onClick.AddListener(() => CloseUI());
        options[5].onClick.AddListener(() => TakeOff());
    }

    private void DisplayTime()
    {
        TickManager tickManager = TickManager.Instance;
        List<string> dialogue = new List<string>();
        dialogue.Add("Current Time is: " + tickManager.Hours + ":" + tickManager.Minutes);
        dialogue.Add("It has been " + tickManager.Days + " days since the crash");
        Dialogue.instance.InitDialogue(dialogue.ToArray());
        Debug.Log("Current Time: " + tickManager.Hours + ":" + tickManager.Minutes);
        CloseUI();
    }

    private void DisplayWeather()
    {
        WeatherManager weatherManager = WeatherManager.Instance;
        List<string> dialogue = new List<string>();
        dialogue.Add("Current Weather is " + weatherManager.GetWeather(weatherManager.CurrentWeather));
        dialogue.Add("I detect " + weatherManager.GetWeather(weatherManager.forecast) + " in the forecast");
        Dialogue.instance.InitDialogue(dialogue.ToArray());
        Debug.Log("Weather: Clear skies");
        CloseUI();
    }

    private void DisplayHints()
    {
        int random = UnityEngine.Random.Range(0, hints.Count);
        Dialogue.instance.InitDialogue(new string[]{hints[random]});
        CloseUI();
    
    }

    private void DisplayRepair()
    {
        List<string> dialogue = new List<string>();
        dialogue.Add("The ship is currently at " + GameManager.getFuel() + "/10 Fuel Rods");
        if (GameManager.getEngine()){
            dialogue.Add("The engine has been inserted");
        } else {
            dialogue.Add("The ship is missing an engine");
        }
        Dialogue.instance.InitDialogue(dialogue.ToArray());
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

    private void TakeOff()
    {
        if (GameManager.getEngine() && GameManager.getFuel() >= 10){
            CloseUI();
            cutsceneManager.PlayFinalCutscene();
        } else {
            List<string> dialogue = new List<string>();
            dialogue.Add("The ship is not ready for takeoff");
            if (!GameManager.getEngine()){
                dialogue.Add("The ship is missing an engine");
            }
            if (GameManager.getFuel() < 10){
                dialogue.Add("The ship is missing fuel");
            }
            Dialogue.instance.InitDialogue(dialogue.ToArray());
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

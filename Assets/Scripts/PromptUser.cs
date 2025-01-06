using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class PromptUser : MonoBehaviour
{
    public static PromptUser instance;
    public TextMeshProUGUI text;
    public string prompt;

    void Awake(){
        if (instance == null){
            instance = this;
        } else{
            Destroy(gameObject);
        }
        text.text = string.Empty;
    }
    public void Reset(){
        text.text = string.Empty;
    }
    public void InitPrompt(string s){
        prompt = s;
        text.text = prompt;
    }
    public void Update(){
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f)){
            if (hit.collider.gameObject.tag == "Interactable"){
                string objName = hit.collider.gameObject.name;
                switch (objName){
                    case "shipConsole":
                        InitPrompt("Interact with the ship AI");
                        break;
                    case "fuelRodContainer":
                        InitPrompt("Deposit fuel rod");
                        break;
                    case "EngineReader":
                        InitPrompt("Insert engine");
                        break;
                    case "CraftingStation":
                        InitPrompt("Use crafting bench");
                        break;
                    case "DoorEntrance":
                        InitPrompt("Enter bunker");
                        break;
                    case "DoorExit":
                        InitPrompt("Exit bunker");
                        break;
                    default:
                        InitPrompt("Read note");
                        break;
                }
            } else {
                Reset();
            }
        } else {
            Reset();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public int storyEvent; // Tracks the current story progression
    public Dialogue dialogueManager; // Reference to the dialogue manager script
    public InventoryManager inventoryManager;
    [SerializeField] Item[] QuestItems;
    private PickUpScript pickUpScript;
    private FuelReader fuelReader;
    private GameObject player;
    private GameManager gameManager;
    private StoryManager storyManager;
    private PromptUser promptManager;

    void Start()
    {
        storyEvent = 0;
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        player = GameObject.Find("Player");
        promptManager = GameObject.Find("PromptBox").GetComponentInChildren<PromptUser>();
        dialogueManager = GameObject.Find("DialogueBox").GetComponentInChildren<Dialogue>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        pickUpScript = Camera.main.GetComponent<PickUpScript>();
        fuelReader = GameObject.Find("FuelStation").GetComponentInChildren<FuelReader>();
        foreach(Item item in QuestItems){
            item.gameObject.SetActive(false);
        }
        QuestItems[2].gameObject.SetActive(true);
    }

    // Called by triggers in the game to update the story progression
    public void AdvanceStoryEvent(int newEvent)
    {
        if (newEvent > storyEvent)
        {
            storyEvent = newEvent;
            HandleEvent(storyEvent);
        }
    }

    // Executes logic based on the current story event
    private void HandleEvent(int eventID)
    {
        List<string> dialogue = new List<string>();
        switch (eventID)
        {
            case 1:
                dialogue.Add("Theres smoke towards the west in the distance");
                dialogue.Add("I should investigate it");
                // Event 1: Prompt to investigate smoke
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 2:
                dialogue.Add("Looks like a severely damaged ship, I doubt there's any survivors");
                dialogue.Add("The cockpit is open, let's see if theres anything useful in there");
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 3:
                dialogue.Add("I found something");
                dialogue.Add("....fuel?");
                dialogue.Add("I can add this to my ships fuel supply");
                dialogueManager.InitDialogue(dialogue.ToArray());
                promptManager.InitPrompt("when an item is held in hand, right click to throw it");
                giveItem(QuestItems[0]);
                break;
            case 4:
                //fuel filled up.
                storyEvent = 5;
                dialogue.Add("I've filled up on fuel, now i need to do something about the engine");
                dialogue.Add("That bunker i saw in the North should have an underground lab");
                dialogue.Add("I can probably find an engine there");
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 5:
                break;
            case 6:
                dialogue.Add("There's a gun on the floor here...");
                dialogue.Add("what could you possibly need a gun for on a deserted planet?");
                promptManager.InitPrompt("when gun is held in hand, left click to shoot bullets");
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 7:
                dialogue.Add("What's that noise...");
                dialogue.Add("Let me find that engine and get out of here ASAP");
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 8:
                if(inventoryManager.ContainsItem(QuestItems[2].ItemName) || ((pickUpScript.getHeldObj != null)&&(player.GetComponentInChildren<Item>()))){
                    dialogue.Add("whew... I made it out alive with the engine");
                    dialogue.Add("Let me put that in my ship and get off this planet!!!");
                } else {
                    dialogue.Add("I need to go back for that engine");
                }
                dialogueManager.InitDialogue(dialogue.ToArray());
                break;
            case 9:
                if(GameManager.getFuel() == 3f && GameManager.getRepair() == 100){
                    promptManager.InitPrompt("YOU SURVIVED!!!");
                }
                break;
            default:
                Debug.Log("Story event not handled: " + eventID);
                break;
        }
    }

    private void giveItem(Item item){
        item.gameObject.SetActive(true);
        pickUpScript.PickUpObject(item.gameObject);
    }

    
}
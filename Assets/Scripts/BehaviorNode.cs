using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
public interface BehaviorNode{
    NodeStatus Execute();
}

public interface nodeAction{
    NodeStatus Execute();
}



public enum NodeStatus{
    Success,
    Failure,
    Running
}

public class BehaviorTree : BehaviorNode {
    List<BehaviorNode> childNodes;
    public void AddChild(BehaviorNode node){
        childNodes.Add(node);
    }
    public BehaviorTree(){
        childNodes = new List<BehaviorNode>();
    }

    public NodeStatus Execute(){
        foreach (var node in childNodes){
            NodeStatus status = node.Execute();
            if (status != NodeStatus.Success){
                return status;
            }
        }
        return NodeStatus.Success;
    }
}

public class SelectorNode : BehaviorNode{
    private List<BehaviorNode> childNodes;
    public SelectorNode(){
        childNodes = new List<BehaviorNode>();
    }
    public void AddChild(BehaviorNode node){
        childNodes.Add(node);
    }
    public NodeStatus Execute()
    {
        foreach (var node in childNodes)
        {
            NodeStatus status = node.Execute();
            if (status == NodeStatus.Success)
            {
                return NodeStatus.Success;
            }
        }
        return NodeStatus.Failure;
    }
}

public class SequenceNode : BehaviorNode
{
    private List<BehaviorNode> childNodes;
    public SequenceNode()
    {
        childNodes = new List<BehaviorNode>();
    }
    public void AddChild(BehaviorNode node){
        childNodes.Add(node);
    }

    public NodeStatus Execute()
    {
        foreach (var node in childNodes)
        {
            NodeStatus status = node.Execute();
            if (status == NodeStatus.Failure)
            {
                return NodeStatus.Failure;
            }
        }
        return NodeStatus.Success;
    }
}


public class ConditionNode : BehaviorNode
{
    private Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public NodeStatus Execute()
    {
        return condition() ? NodeStatus.Success : NodeStatus.Failure;
    }
}

public class ActionNode : BehaviorNode
{
    private nodeAction action;

    public ActionNode(nodeAction action)
    {
        this.action = action;
    }

    public NodeStatus Execute() => action.Execute();
}

public class DisplayOptionsAction : nodeAction
{
    private GameObject uiPanel;
    private string text;
    private string[] options;
    private List<Button> buttons;

    public DisplayOptionsAction(GameObject uiPanel, string text, string[] options, List<Button> buttonsList){
        this.uiPanel = uiPanel;
        this.text = text;
        this.options = options;
        this.buttons = buttonsList;
    }

    public NodeStatus Execute()
    {
        // Display the text
        Text uiText = uiPanel.GetComponentInChildren<Text>();
        if (uiText != null){
            uiText.text = text;
        }

        GameObject dialogue = uiPanel.transform.Find("dialogue").gameObject;
        Dialogue dialogueManager = dialogue.GetComponent<Dialogue>();
        if (dialogueManager != null){
            dialogueManager.InitDialogue(new string[] { "Hello, how can i help you" });
        }

        // Display the buttons
        for (int i = 0; i < buttons.Count && i < options.Length; i++){
            buttons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            buttons[i].gameObject.SetActive(true);
        }

        // Unlock the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        return NodeStatus.Success;
    }
}
public class WaitForOptionAction : nodeAction{
    private GameObject uiPanel;
    private string selectedOption;
    private bool optionSelected;
    private List<Button> options;

    public WaitForOptionAction(GameObject uiPanel, List<Button> buttons)
    {
        this.uiPanel = uiPanel;
        this.options = buttons;
        this.optionSelected = false;
    }

    public NodeStatus Execute()
    {
        if (!optionSelected)
        {
            // Enable the UI and unlock the cursor
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Clear previous listeners to prevent stacking issues
            foreach (Button button in options)
            {
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(true);

                // FIX: Capture the button text in a local variable properly to avoid closure issues
                string buttonText = button.GetComponentInChildren<Text>().text;
                
                // Add the listener with the correctly captured variable
                button.onClick.AddListener(() => OnOptionSelected(buttonText));
            }
            return NodeStatus.Running;
        }
        else
        {
            // Disable the buttons and reset the cursor when selection is complete
            foreach (Button button in options)
            {
                button.gameObject.SetActive(false);
            }

            ProcessSelectedOption();
            return NodeStatus.Success;
        }
    }

    private void OnOptionSelected(string option)
    {
        // Ensure this method sets the state only once
        if (!optionSelected)
        {
            selectedOption = option;
            optionSelected = true;
            Debug.Log("Option Selected: " + selectedOption);
        }
    }

    private void ProcessSelectedOption()
    {
        Debug.Log("Processing Selected Option: " + selectedOption);

        // Lock the cursor back after selection
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the UI Panel completely
        uiPanel.SetActive(false);

        // Reset state for next use of the node (optional if you want it reusable)
        optionSelected = false;
        selectedOption = null;
    }
}

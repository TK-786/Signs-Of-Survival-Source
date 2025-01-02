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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Button[] buttons = uiPanel.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => OnOptionSelected(button.GetComponentInChildren<Text>().text));
            }
            return NodeStatus.Running;
        }
        else
        {
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
        selectedOption = option;
        optionSelected = true;
    }

    private void ProcessSelectedOption()
    {
        Debug.Log("Selected option: " + selectedOption);
        // Add your logic to process the selected option here
        
        // Lock the mouse cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the UI panel
        uiPanel.SetActive(false);
    }
}

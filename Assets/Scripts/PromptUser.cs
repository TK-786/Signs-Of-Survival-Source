using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromptUser : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string prompt;
    private float timer;
    void Start(){
        text.text = string.Empty;
    }

    void Update(){
        timer += Time.deltaTime;
        if (timer >= 6f){
            timer = 0f;
            text.text = string.Empty;
        }
    }
    public void InitPrompt(string s){
        prompt = s;
        timer = 0;
        text.text = prompt;
    }
}

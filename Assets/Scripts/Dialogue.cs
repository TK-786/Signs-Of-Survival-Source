using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour{
    public TextMeshProUGUI text;
    public string[] lines;
    private int index;
    private bool typing;
    private float timer;
    void Start(){
        text.text = string.Empty;
        typing = false;
    }

    void Update(){
        if (!typing){
            timer += Time.deltaTime;
            if (timer >= 3f){
                timer = 0f;
                NextLine();
            }
        }
    }

    public void InitDialogue(string[] dialogue){
        lines = dialogue;
        index = 0;
        typing = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        foreach(char c in lines[index].ToCharArray()){
            text.text += c;
            yield return new WaitForSeconds(0.05f );
        }
        typing = false;
    }
    void NextLine(){
        if (index < lines.Length - 1){
            index++;
            text.text = string.Empty;
            typing = true;
            StartCoroutine(TypeLine());
        }

        else{
            text.text = string.Empty;
        }
    }
}
using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    [SerializeField] private float tickFrequency = 1f;
    private static int currentTick = 0;
    private float lastTickTime = 0;
    [SerializeField] private static float currentTime;
    public static Action OnTick;

    private void Awake(){
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update(){
        Tick();
    }
    
    private void Tick(){
        currentTime += Time.deltaTime;
        if (currentTime >= lastTickTime + tickFrequency){
            lastTickTime = currentTime;
            OnTick?.Invoke();
            currentTick++;
        }
    }
}
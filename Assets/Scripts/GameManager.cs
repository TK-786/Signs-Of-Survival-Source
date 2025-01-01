using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Variables to track fuel and repair status
    public static float fuel = 0f;
    public static float repair = 0f;

    // Method to increment the fuel level, up to a maximum of 3
    public static void IncrementFuel()
    {
        if (fuel < 3f) { fuel++; }
    }

    // Method to decrement the fuel level
    public static void DecrementFuel()
    {
        fuel--;
    }

    // Method to set the repair status to fully repaired (100)
    public static void RepairedShip()
    {
        repair = 100;
    }

    // Method to set the repair status to broken (0)
    public static void BreakShip()
    {
        repair = 0;
    }

    // Method to get the current repair status
    public static float getRepair()
    {
        return repair;
    }

    // Method to get the current fuel level
    public static float getFuel()
    {
        return fuel;
    }

    public void LoadNextScene(string nextSceneName)
    {
        SceneManager.LoadSceneAsync(nextSceneName).completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        Debug.Log("Scene loaded, syncing inventory...");
        GameObject spawnPoint = GameObject.Find("SpawnPosition");
        PlayerController.Instance.transform.position = spawnPoint.transform.position;
    }    
}
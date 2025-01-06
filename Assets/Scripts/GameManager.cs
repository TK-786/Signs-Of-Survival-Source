using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string lastSceneName;
    private string spawn;

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
    public static bool Engine = false;

    // Method to increment the fuel level, up to a maximum of 3
    public static void IncrementFuel()
    {
        if (fuel < 10f) { fuel++; }
    }

    // Method to decrement the fuel level
    public static void DecrementFuel()
    {
        fuel--;
    }

    public static void submitEngine(){
        Engine = true;
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

    public static bool getEngine(){
        return Engine;
    }

    public void LoadNextScene(string nextSceneName, string spawnPointName = "SpawnPosition")
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        spawn = spawnPointName;
        SceneManager.LoadSceneAsync(nextSceneName).completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        Debug.Log("Scene loaded, syncing inventory...");
        StartCoroutine(SetPlayerPosition());
    }

    private System.Collections.IEnumerator SetPlayerPosition()
    {
        // Wait until the SpawnPosition object is found
        GameObject spawnPoint = null;
        int retries = 10; // Number of attempts to find the spawn point
        float retryDelay = 0.1f; // Delay between attempts

        while (spawnPoint == null && retries > 0)
        {
            spawnPoint = GameObject.Find(spawn);
            if (spawnPoint == null)
            {
                retries--;
                Debug.LogWarning("SpawnPosition not found yet, retrying...");
                yield return new WaitForSeconds(retryDelay); // Wait before retrying
            }
        }

        if (spawnPoint != null)
        {
            Debug.Log($"Found spawn point at: {spawnPoint.transform.position}");

            // Access the player and its CharacterController
            PlayerController player = PlayerController.Instance;
            CharacterController controller = player.GetComponent<CharacterController>();

            // Disable CharacterController to avoid conflicts
            controller.enabled = false;

            // Set player position
            player.transform.position = spawnPoint.transform.position;

            // Re-enable CharacterController
            controller.enabled = true;

            Debug.Log($"Player moved to spawn point at: {spawnPoint.transform.position}");
        }
        else
        {
            Debug.LogError("SpawnPoint not found after retries!");
        }
    }
}
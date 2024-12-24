using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; 
    public GameObject settingsMenu;

    private bool gamePaused = false;
    private Stack<GameObject> PauseMenuStackHistory = new Stack<GameObject>();
    public GameObject player;

    void Start()
    {
        Debug.Log("PauseMenu initialized.");
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void Pause()
    {
        Debug.Log("Pause method triggered.");
        if (gamePaused)
        {
            if (GetCurrentPauseMenu() != pauseMenu)
            {
                return;
            }
            else
            {
                ResumeGame();
            }
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame called. Activating pause menu.");

        if (pauseMenu == null)
        {
            Debug.LogError("pauseMenu is not assigned!");
            return;
        }

        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f;       // Freeze all game logic
        gamePaused = true;

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;

        // Disable player input
        player.GetComponent<PlayerController>().isPaused = true;

        PauseMenuStackHistory.Clear();
        PauseMenuStackHistory.Push(pauseMenu);
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame called. Deactivating pause menu.");

        if (pauseMenu == null)
        {
            Debug.LogError("pauseMenu is not assigned!");
            return;
        }
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f;        
        gamePaused = false;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        // Enable player input
        player.GetComponent<PlayerController>().isPaused = false;
    }



    public void OpenSettingsMenu()
    {
        Debug.Log("Settings menu opened.");
        if (GetCurrentPauseMenu() != null)
        {
            PauseMenuStackHistory.Push(GetCurrentPauseMenu());
        }

        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    private GameObject GetCurrentPauseMenu()
    {
        if (pauseMenu.activeSelf) return pauseMenu;
        if (settingsMenu.activeSelf) return settingsMenu;
        return null;
    }

    public void BackButton()
    {
        Debug.Log("BackButton pressed. Stack count: " + PauseMenuStackHistory.Count);

        if (PauseMenuStackHistory.Count > 0)
        {
            GameObject currMenu = GetCurrentPauseMenu();
            if (currMenu != null)
            {
                currMenu.SetActive(false);
            }

            GameObject lastMenu = PauseMenuStackHistory.Pop();
            lastMenu.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }
}

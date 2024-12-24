using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class mainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject settingsMenu;
    public GameObject playGameMenu;
    public GameObject difficultyMenu;

    //difficulty Menu buttons
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button chaosButton;

    //chaos Menu overlay
    public GameObject chaosMenu;

    public GameObject gameTitle;

    //this creates a stack with menu history so when I call back function, I dont
    //manually create a new function depending on where the back function is being
    //called in the menu stack. I use this instead.
    private Stack<GameObject> menuStackHistory = new Stack<GameObject>();



    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 

        MainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        playGameMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        gameTitle.SetActive(true);
    }


    private GameObject GetCurrentMenu()
    {
        if (MainMenu.activeSelf) return MainMenu;
        if (settingsMenu.activeSelf) return settingsMenu;
        if (playGameMenu.activeSelf) return playGameMenu;
        if (difficultyMenu.activeSelf) return difficultyMenu;
        return null;
    }
    public void PlayGame()
    {
        MainMenu.SetActive(false);
        playGameMenu.SetActive(true);
        gameTitle.SetActive(false);
        //MainMenu is current menu shown, so push this into stack
        menuStackHistory.Push(MainMenu);

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void OpenNewGame()
    {
        playGameMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        //playGameMenu is current menu, so push onto stack
        menuStackHistory.Push(playGameMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        MainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        gameTitle.SetActive(false);
        //MainMenu is current menu shown, so push this into stack
        menuStackHistory.Push(MainMenu);

    }


    public void LoadChaosMenu()
    {
        LoadGame();
        Time.timeScale = 0;
        if (chaosMenu != null)
        {
            chaosMenu.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

    }



    public void BackButton()
    {
        if (menuStackHistory.Count > 0)
        {
            GameObject lastMenu = menuStackHistory.Pop();

            GetCurrentMenu().SetActive(false);
            lastMenu.SetActive(true);

            if (lastMenu == MainMenu)
            {
                gameTitle.SetActive(true);
            }
            else
            {
                gameTitle.SetActive(false);
            }
        }
        else
        {
            MainMenu.SetActive(true);
            gameTitle.SetActive(true);
        }

        // Ensure the mouse is unlocked and visible when returning to menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

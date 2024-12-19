using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    //private int savedGameInd;
    private bool gamePaused = false;
    private Stack<GameObject> menuStackHistory = new Stack<GameObject>();

    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;

    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        //settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    // Update is called once per frame
    void Update()
    {
        Debug.Log("pressing esc");
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("gamePaused: " + gamePaused);
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }


        }
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        menuStackHistory.Push(pauseMenu);


    }

    public void BackButton()
    {

        if (menuStackHistory.Count > 0)
        {

            GameObject lastMenu = menuStackHistory.Pop();
            lastMenu.SetActive(true);
            settingsMenu.SetActive(false);

        }
    } 
}

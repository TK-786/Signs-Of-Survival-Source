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
    private Stack<GameObject> PauseMenuStackHistory = new Stack<GameObject>();

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
        PauseMenuStackHistory.Clear();
        PauseMenuStackHistory.Push(pauseMenu);

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
        if (pauseMenu.activeSelf || settingsMenu.activeSelf)
        {
            return; 
        }
      
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
        if (GetCurrentPauseMenu() != null)
            PauseMenuStackHistory.Push(GetCurrentPauseMenu());
        {

            settingsMenu.SetActive(true);
            pauseMenu.SetActive(false);



        }
    }
    private GameObject GetCurrentPauseMenu()
    {
        if (pauseMenu.activeSelf) return pauseMenu;
        if (settingsMenu.activeSelf) return settingsMenu;
        return null;
    }

    public void BackButton()
    {

        if (PauseMenuStackHistory.Count > 0)
        {
            GameObject currMenu = GetCurrentPauseMenu();
            if (currMenu != null)
            {
                currMenu.SetActive(false);
            }

            GameObject lastMenu = PauseMenuStackHistory.Pop();
            //GetCurrentPauseMenu().SetActive(false);
            lastMenu.SetActive(true);

        }
        else
        {
            ResumeGame();
        }
    }
}

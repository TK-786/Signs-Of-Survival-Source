using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject settingsMenu;
public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
   
    public void quitGame()
    {
        Application.Quit();
    }

    public void openSettings()
    {
        MainMenu.SetActive(false);
        settingsMenu.SetActive(true);

    }
    public void closeSettings()
    {
        MainMenu.SetActive(true);
        settingsMenu.SetActive(false);

    }

}

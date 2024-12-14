using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class mainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject settingsMenu;
    public GameObject playGameMenu;
    public GameObject difficultyMenu;
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public TMP_Dropdown difficultySetting;
    public Image brightnessOverlay;
    public GameObject gameTitle;

    //this creates a stack with menu history so when I call back function, I dont
    //manually create a new function depending on where the back function is being
    //called in the menu stack. I use this instead.
    private Stack<GameObject> menuStackHistory = new Stack<GameObject>();



    private void Start()
    {
        
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 2f);
        brightnessSlider.onValueChanged.AddListener(ChangeBrightnessFunc);

        difficultySetting.value = PlayerPrefs.GetInt("Difficulty", 1);
        difficultySetting.onValueChanged.AddListener(ChangeDifficultyFunc);
    }
    //this function adds the menu to the stack by calling push function
    public void OpenMenu(GameObject newMenu)
    {
        if (menuStackHistory.Count > 0)  // If there's a menu to go back to, push the current one onto the stack
        {
            menuStackHistory.Push(GameObject.FindGameObjectWithTag("currMenu"));
        }

        //this hides the curr menu and shows the new one when popped.
        newMenu.SetActive(true);
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
    public void CloseSettings()
    {
        MainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        gameTitle.SetActive(true);

    }
    public void ChangeVolume(float v)
    {
        AudioListener.volume = v;
    }

    public void ChangeBrightnessFunc(float b)
    {
        PlayerPrefs.SetFloat("Brightness", b);
        PlayerPrefs.Save();
        Color color = brightnessOverlay.color;
        color.a = 1 - b;
        brightnessOverlay.color = color;


    }
    public void ChangeDifficultyFunc(int d)
    {
        PlayerPrefs.SetInt("Difficulty", d);
        PlayerPrefs.Save();
    }

    public void BackButton()
    {
        if (menuStackHistory.Count > 0)
        {
            GameObject lastMenu = menuStackHistory.Pop();
            lastMenu.SetActive(true);
        }
    }
}

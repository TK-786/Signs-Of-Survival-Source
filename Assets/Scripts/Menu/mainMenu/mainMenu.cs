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
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public TMP_Dropdown difficultySetting;
    public Image brightnessOverlay;
    public GameObject gameTitle;


    private void Start()
    {
        
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 2f);
        brightnessSlider.onValueChanged.AddListener(ChangeBrightnessFunc);

        difficultySetting.value = PlayerPrefs.GetInt("Difficulty", 1);
        difficultySetting.onValueChanged.AddListener(ChangeDifficultyFunc);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
}

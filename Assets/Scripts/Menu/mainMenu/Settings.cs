using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;

public class Settings : MonoBehaviour
{
    //public Slider volumeSlider;
    //public Slider brightnessSlider;
    //public Image brightnessOverlay;

    public GameObject basicSettings;
    public GameObject keyBindsSettings;

    public GameObject basicButton;
    public GameObject keyBindsButton;

    private Vector3 basicButtonOgPosition;
    private Vector3 keyBindsButtonOgPosition;

    public float popOutAmount = 20f; 
    public float popOutSpeed = 5f;

    private GameObject currentActiveButton;


    // Start is called before the first frame update
    //void Start()
    //{
    //    volumeSlider.value = AudioListener.volume;
    //    volumeSlider.onValueChanged.AddListener(ChangeVolume);

    //    brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 2f);
    //    brightnessSlider.onValueChanged.AddListener(ChangeBrightnessFunc);

    //}

    //public void ChangeVolume(float v)
    //{
    //    AudioListener.volume = v;
    //}

    //public void ChangeBrightnessFunc(float b)
    //{
    //    PlayerPrefs.SetFloat("Brightness", b);
    //    PlayerPrefs.Save();
    //    Color color = brightnessOverlay.color;
    //    color.a = 1 - b;
    //    brightnessOverlay.color = color;


    //}

    private void Start()
    {
        basicButtonOgPosition = basicButton.transform.localPosition;
        keyBindsButtonOgPosition = keyBindsButton.transform.localPosition;

        ShowBasicSettings();

    }

    public void ShowBasicSettings()
    {
        basicSettings.SetActive(true);
        keyBindsSettings.SetActive(false);

        ButtonPopOut(basicButton);
        ResetButton(keyBindsButton);

        currentActiveButton = basicButton;


    }
    public void ShowKeyBindsSettings()
    {
        basicSettings.SetActive(false);
        keyBindsSettings.SetActive(true);

        ButtonPopOut(keyBindsButton);
        ResetButton(basicButton);

        currentActiveButton = keyBindsButton;
    }

    private void ButtonPopOut(GameObject clickedButton)
    {
        if (clickedButton != null)
        {
           
            clickedButton.transform.localPosition = new Vector3(
                clickedButton == basicButton ? basicButtonOgPosition.x - popOutAmount : keyBindsButtonOgPosition.x - popOutAmount,
                clickedButton.transform.localPosition.y,
                clickedButton.transform.localPosition.z
            );
        }

    }
    private void ResetButton(GameObject resetButton) 
    {
        if (resetButton != null)
        {
            Vector3 targetPosition = resetButton == basicButton ? basicButtonOgPosition : keyBindsButtonOgPosition;
            resetButton.transform.localPosition = targetPosition;
        }
    }

}

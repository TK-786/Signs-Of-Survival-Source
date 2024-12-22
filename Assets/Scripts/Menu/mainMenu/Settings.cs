using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    //public Slider volumeSlider;
    //public Slider brightnessSlider;
    //public Image brightnessOverlay;

    public UnityEngine.UI.Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;


    [SerializeField] private string mainMenuCanvasName = "startMenu";

    public GameObject basicSettings;
    public GameObject keyBindsSettings;

    public GameObject basicButton;
    public GameObject keyBindsButton;

    private Vector3 basicButtonOgPosition;
    private Vector3 keyBindsButtonOgPosition;

    public float popOutAmount = 20f; 
    public float popOutSpeed = 5f;

    private Canvas mainMenuCanvas;

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

        float sensitivityValue = PlayerPrefs.GetFloat("Sensitivity", 7);
        sensitivitySlider.value = sensitivityValue;
       
        if (sensitivityText != null)
        {
            sensitivityText.text = $"{sensitivityValue}";
        }
        else
        {
            Debug.LogError("sensitivityText is not assigned in the Inspector!");
        }

        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        ChangeSensitivity(sensitivityValue);

        basicButtonOgPosition = basicButton.transform.localPosition;
        keyBindsButtonOgPosition = keyBindsButton.transform.localPosition;

        mainMenuCanvas = CanvasName(mainMenuCanvasName);
        ShowBasicSettings();

    }

    public void ChangeSensitivity(float changedVal)
    {

        if (sensitivityText != null)
        {
            sensitivityText.text = $"{changedVal}";
        }
        else
        {
            Debug.LogError("sensitivityText is not assigned in the Inspector!");
            return;
            
        }
        PlayerPrefs.SetFloat("Sensitivity", changedVal);

    }

    public void ShowBasicSettings()
    {
        basicSettings.SetActive(true);
        keyBindsSettings.SetActive(false);

        if (IsInMainMenuCanvas())
        {
            ButtonPopOutMain(basicButton);

        }
        else
        {
            ButtonPopOutPause(basicButton);
        }

        ResetButton(keyBindsButton);
        //currentActiveButton = basicButton;


    }
    public void ShowKeyBindsSettings()
    {
        basicSettings.SetActive(false);
        keyBindsSettings.SetActive(true);
        if (IsInMainMenuCanvas())
        {
            ButtonPopOutMain(keyBindsButton);

        }
        else
        {
            ButtonPopOutPause(keyBindsButton);
        }

        ResetButton(basicButton);
        //currentActiveButton = keyBindsButton;
    }

    private void ButtonPopOutPause(GameObject clickedButton)
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
    private void ButtonPopOutMain(GameObject clickedButton)
    {

        if (clickedButton != null && clickedButton.transform.localPosition.y != basicButtonOgPosition.y + popOutAmount)
        {
                clickedButton.transform.localPosition = new Vector3(
                    clickedButton.transform.localPosition.x,
                    clickedButton.transform.localPosition.y + popOutAmount,
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

    private bool IsInMainMenuCanvas()
    {
        return mainMenuCanvas != null && mainMenuCanvas.gameObject.activeSelf;
    }

    private Canvas CanvasName(string canvasName)
    {
        Canvas[] Canvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in Canvases)
        {
            if (canvas.name == canvasName)
            {
                return canvas;
            }
        }
        return null;
    }
}

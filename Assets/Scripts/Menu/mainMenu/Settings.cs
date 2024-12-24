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
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Slider brightnessSlider;
    public UnityEngine.UI.Image brightnessOverlay;

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



    private void Start()
    {
        //volumeSlider.value = AudioListener.volume;
        //volumeSlider.onValueChanged.AddListener(ChangeVolume);


        //this is for the brightness settings. maybe import this to scene script too
        if (brightnessOverlay != null)
        {
            Color color = brightnessOverlay.color;
            color.a = 0; // Set to 0 transparency
            brightnessOverlay.color = color;
        }
        brightnessSlider.value = 0;
        PlayerPrefs.SetFloat("Brightness", 0); 
        PlayerPrefs.Save();




        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); 
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);


        float sensitivityValue = PlayerPrefs.GetFloat("Sensitivity", 7);
        sensitivitySlider.value = sensitivityValue;
        if (sensitivityText != null)
        {
            sensitivityText.text = $"{sensitivityValue}";
        }
      

        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        ChangeSensitivity(sensitivityValue);

        basicButtonOgPosition = basicButton.transform.localPosition;
        keyBindsButtonOgPosition = keyBindsButton.transform.localPosition;

        mainMenuCanvas = CanvasName(mainMenuCanvasName);
        ShowBasicSettings();

    }

    public void ChangeVolume(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("Volume", v);
        PlayerPrefs.Save();
    }

    public void ChangeBrightnessFunc(float b)
    {
        PlayerPrefs.SetFloat("Brightness", b);
        PlayerPrefs.Save();
        if (brightnessOverlay != null)
        {
            Color color = brightnessOverlay.color;
            //color.a = 1 - b;
            //color.a = Mathf.Clamp(1 - b, 0, 0.7f);
            color.a = Mathf.Lerp(0, 0.7f, b);
            brightnessOverlay.color = color;
        }



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
            Debug.Log("here in basic");
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
            Debug.Log($"Original Position: {clickedButton.transform.localPosition}, Target Position: {clickedButton.transform.localPosition}");
            clickedButton.transform.localPosition = clickedButton.transform.localPosition;
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

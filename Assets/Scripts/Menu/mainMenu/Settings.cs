using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public UnityEngine.UI.Slider volumeSlider;

    public UnityEngine.UI.Slider brightnessSlider;
    public UnityEngine.UI.Image brightnessOverlay;

    private float sensitivityValue;

    public TextMeshProUGUI sensitivityText;

    public UnityEngine.UI.Slider resolutionSlider;

    public UnityEngine.UI.Toggle visualSoundEffects;



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

    private PlayerController player;
    [SerializeField] private GameObject playerObj;

    private void Start()
    {
        
        // sensitivitySlider.minValue = 0.1f;
        //this sets the volume settings
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f); 
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        //this sets the brightness settings
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f); 
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness); 
        brightnessSlider.onValueChanged.AddListener(ChangeBrightnessFunc);

        //this sets the sensitivity settings
        sensitivityValue = PlayerPrefs.GetFloat("Sensitivity", 7f);
        if (sensitivityText != null)
        {
            sensitivityText.text = $"{sensitivityValue}";
        }

        //this is for resolutions settings
        float savedResolution = PlayerPrefs.GetFloat("Resolution", 0f);
        resolutionSlider.value = savedResolution;
        SetResolution(savedResolution);
        //brightnessSlider.onValueChanged.AddListener(ChangeResolution);

        //this is for resolutions settings
        int savedVisualSoundEffects = PlayerPrefs.GetInt("VisualSoundEffects", 0);
        visualSoundEffects.isOn = (savedVisualSoundEffects == 1);
        SetVisualSoundEffects(visualSoundEffects.isOn);
        visualSoundEffects.onValueChanged.AddListener(ChangeVisualSoundEffects);



        basicButtonOgPosition = basicButton.transform.localPosition;
        keyBindsButtonOgPosition = keyBindsButton.transform.localPosition;

        mainMenuCanvas = CanvasName(mainMenuCanvasName);
        //pauseMenuCanvas = CanvasName(pauseMenuCanvasName);
        ShowBasicSettings();

        if (playerObj != null){
            player = playerObj.GetComponent<PlayerController>();
        }

    }



    public void ChangeVolume(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("Volume", v);
        PlayerPrefs.Save();
    }


    public void ChangeBrightnessFunc(float b)
    {
        SetBrightness(b);
        PlayerPrefs.SetFloat("Brightness", b);
        PlayerPrefs.Save();
        
    }
    public void SetBrightness(float b)
    {
        if (brightnessOverlay != null)
        {
            Color color = brightnessOverlay.color;
            color.a = Mathf.Lerp(0, 0.7f, b);
            brightnessOverlay.color = color;
        }
    }


    public void IncreaseSensitivity()
    {
        sensitivityValue += 0.1f;  
        sensitivityValue = Mathf.Clamp(sensitivityValue, 0.1f, 10f);  
        UpdateSensitivityText();
        if (player != null)
        {
            player.mouseSensitivity = sensitivityValue;
        }
    }

    public void DecreaseSensitivity()
    {
        sensitivityValue -= 0.1f;  // Decrease by 0.1
        sensitivityValue = Mathf.Clamp(sensitivityValue, 0.1f, 10f); 
        UpdateSensitivityText();
        if (player != null)
        {
            player.mouseSensitivity = sensitivityValue;
        }
    }

    private void UpdateSensitivityText()
    {
        sensitivityText.text = $"{sensitivityValue:F2}";
    }


    public void ChangeResolution(float r)
    {
        SetResolution(r);
        PlayerPrefs.SetFloat("Resolution", r);
        PlayerPrefs.Save();

    }
    private void SetResolution(float r)
    {
        //default resolution values
        int w = 1921;
        int h = 1080;

        if (r < 0.33f)
        {
            w = 1280;
            h = 720;
        }
        else if(r > 0.33f & r < 0.66f)
        {
            w = 1600;
            h = 900;
        }
        Screen.SetResolution(w, h, FullScreenMode.Windowed);
    }

    public void ChangeVisualSoundEffects(bool ticked)
    {
        PlayerPrefs.SetInt("VisualSoundEffects", ticked ? 1 : 0);
        PlayerPrefs.Save();
        SetVisualSoundEffects(ticked);
    }
    public void SetVisualSoundEffects(bool ticked)
    {
        if (ticked)
        {
            Debug.Log("Visual sound effects enabled.");

            //visual sound effects is enabled
        }
        else
        {
            Debug.Log("Visual sound effects disabled.");

            //visual sound effects disabled
        }

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










    //this is now for starting a new game, in which you would want settings to be reset.

    public void StartGameWithResetSettings()
    {
        resetBrightness();
        resetVolume();
        resetSensitivity();

    }



    public void resetBrightness()
    {
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
    }

    public void resetVolume()
    {
        AudioListener.volume = 1f;
        PlayerPrefs.SetFloat("Volume", 1f);
        PlayerPrefs.Save();

    }
    public void resetSensitivity()
    {
        sensitivityValue = 7f;
        PlayerPrefs.SetFloat("Sensitivity", sensitivityValue);
        PlayerPrefs.Save();

        // Update UI
        if (player != null)
        {
            player.mouseSensitivity = sensitivityValue;
        }
    }
}

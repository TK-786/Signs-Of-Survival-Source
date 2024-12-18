using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Image brightnessOverlay;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 2f);
        brightnessSlider.onValueChanged.AddListener(ChangeBrightnessFunc);

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

}

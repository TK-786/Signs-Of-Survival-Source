using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    [SerializeField] private float tickFrequency = 1f;
    private static int currentTick = 0;
    private float lastTickTime = 0;
    [SerializeField] private static float currentTime;
    public static Action OnTick;

    [SerializeField] private Cubemap skyBoxNight;
    [SerializeField] private Cubemap skyBoxSunrise;
    [SerializeField] private Cubemap skyBoxDay;
    [SerializeField] private Cubemap skyBoxSunset;

    private int minutes;
    public int Minutes
    { get {return minutes;} set {minutes = value; OnMinutesChange(value);} }

    private int hours;
    public int Hours
    { get {return hours;} set {hours = value; OnHoursChange(value);} }

    private int days;
    public int Days
    { get { return days; } set { days = value; } }
 
    private void Awake(){
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        RenderSettings.skybox.SetTexture("_Texture1", skyBoxNight);
        RenderSettings.skybox.SetTexture("_Texture2", skyBoxNight); // Ensure seamless transition
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }

    private void Update()
    {
        Tick();
    }
    
    private void Tick(){
        currentTime += Time.deltaTime;
        if (currentTime >= lastTickTime + tickFrequency){
            lastTickTime = currentTime;
            OnTick?.Invoke();
            currentTick++;
            Minutes += 1;
            //Debug.Log("Tick happened, current hours: " + Hours + " current minutes: " + Minutes);
        }
        
    }

    private void OnMinutesChange(int value)
    {
        if (value >= 5) {
            Hours += 1;
            minutes = 0;
        }
        if (Hours >= 24) {
            Days += 1;
            Hours = 0;
        }
    }

    private void OnHoursChange(int value)
    {
        Debug.Log("Hour Changed: " + value);
        if (value == 5){ // Night to Sunrise
            StartCoroutine(LerpSkybox(skyBoxNight, skyBoxSunrise, 30f));
        }
        else if (value == 8){ // Sunrise to Day
            StartCoroutine(LerpSkybox(skyBoxSunrise, skyBoxDay, 30f));
        }
        else if (value == 17){ // Day to Sunset
            StartCoroutine(LerpSkybox(skyBoxDay, skyBoxSunset, 30f));
        }
        else if (value == 22){ // Sunset to Night
            StartCoroutine(LerpSkybox(skyBoxSunset, skyBoxNight, 20f));
        }

    }

    private IEnumerator LerpSkybox(Cubemap a, Cubemap b, float time)
    {
        Debug.Log("Starting skybox transition.");
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
}
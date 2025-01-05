using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;
    private int period = 10;
    private int weatherTick = 0;
    [SerializeField] private Weather currentWeather = Weather.clear;
    public Weather CurrentWeather => currentWeather;
    public GameObject Player;
    private Queue<Weather> weatherQueue;
    public Weather forecast => weatherQueue.Peek();
    [SerializeField] ParticleSystem rainPS;
    [SerializeField] ParticleSystem snowPS;
    [SerializeField] ParticleSystem lightningPS;
    [SerializeField] AudioClip rainSound;
    [SerializeField] AudioClip thunderSound;
    [SerializeField] AudioClip rainLightningSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
        audioSource = gameObject.GetComponent<AudioSource>();
        period = 10;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = Color.grey;
        RenderSettings.fogDensity = 0.005f;
        rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        FillQueue();
        changeWeather();
    }

    public string GetWeather(Weather weather){
        switch(weather){
            case Weather.clear:
                return "Clear skies";
            case Weather.foggy:
                return "Foggy weather";
            case Weather.rainy:
                return "Rain";
            case Weather.lightning:
                return "Lightning";
            case Weather.rain_lightning:
                return "Rain and Lightning";
            case Weather.snow:
                return "Snow";
            default:
                return "Clear skies";
        }
    }

    private void Update(){
        Vector3 playerPos = Player.transform.position;
        rainPS.transform.position = new Vector3(playerPos.x, 30, playerPos.z - 10);
        snowPS.transform.position = new Vector3(playerPos.x, 30, playerPos.z - 10);
        lightningPS.transform.position = new Vector3(playerPos.x, lightningPS.transform.position.y, playerPos.z);
    }

    private void OnEnable(){
        TickManager.OnTick += Tick;
    }

    private void OnDisable(){
        TickManager.OnTick -= Tick;
    }

    private void Tick(){
        weatherTick++;
        Debug.Log("gameTick: " + TickManager.CurrentTick + ", weatherTick: " + weatherTick);
        if(weatherTick >= period){
            weatherTick = 0;
            changeWeather();
        }
    }

    void FillQueue(){
        weatherQueue = new Queue<Weather>();
        for(int i = 0; i < 3; i++){
            weatherQueue.Enqueue((Weather)UnityEngine.Random.Range(0,(int)Weather.max));
        }
    }

    void changeWeather(){
        currentWeather = weatherQueue.Dequeue();
        weatherQueue.Enqueue((Weather)UnityEngine.Random.Range(0,(int)Weather.max));
        int densityMultiplier = UnityEngine.Random.Range(5, 16);
        Debug.Log("current weather: " + currentWeather);
        switch(currentWeather){
            case Weather.clear:
                period = 360;
                audioSource.Stop();
                RenderSettings.fogDensity = 0.005f;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.foggy:
                period = 360;
                audioSource.Stop();
                RenderSettings.fogDensity = 0.001f * densityMultiplier;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.rainy:
                period = 360;
                audioSource.Stop();
                audioSource.clip = rainSound;
                audioSource.loop = true;
                audioSource.Play();
                RenderSettings.fogDensity = 0.005f;
                rainPS.Play();
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.lightning:
                period = 360;
                audioSource.Stop();
                audioSource.clip = thunderSound;
                audioSource.loop = true;
                audioSource.Play();
                RenderSettings.fogDensity = 0.005f;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Play();
                break;
            case Weather.rain_lightning:
                period = 360;
                audioSource.Stop();
                audioSource.clip = rainLightningSound;
                audioSource.loop = true;
                audioSource.Play();
                RenderSettings.fogDensity = 0.005f;
                rainPS.Play();
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Play();
                break;
            case Weather.snow:
                period = 360;
                RenderSettings.fogDensity = 0.005f;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Play();
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
        }
    }
}

public enum Weather{
    clear,
    foggy,
    rainy,
    lightning,
    rain_lightning,
    snow,
    max
}
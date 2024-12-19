using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    private int period = 10;
    private int weatherTick = 0;
    [SerializeField] private Weather currentWeather = Weather.clear;
    public Weather CurrentWeather => currentWeather;
    public GameObject Player;
    private Queue<Weather> weatherQueue;
    [SerializeField] ParticleSystem rainPS;
    [SerializeField] ParticleSystem snowPS;
    [SerializeField] ParticleSystem lightningPS;
    [SerializeField] ParticleSystem fogPS;
    // Start is called before the first frame update
    void Start()
    {
        period = 10;
        rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        FillQueue();
        changeWeather();
    }

    private void Update(){
        Vector3 playerPos = Player.transform.position;
        rainPS.transform.position = new Vector3(playerPos.x, 30, playerPos.z - 10);
        snowPS.transform.position = new Vector3(playerPos.x, 30, playerPos.z - 10);
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
        Debug.Log("current weather: " + currentWeather);
        switch(currentWeather){
            case Weather.clear:
                period = 2;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.foggy:
                period = 2;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fogPS.Play();
                break;
            case Weather.rainy:
                period = 100;
                rainPS.Play();
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.lightning:
                period = 2;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Play();
                fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.rain_lightning:
                period = 2;
                rainPS.Play();
                snowPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                lightningPS.Play();
                fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                break;
            case Weather.snow:
                period = 100;
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                snowPS.Play();
                lightningPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fogPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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
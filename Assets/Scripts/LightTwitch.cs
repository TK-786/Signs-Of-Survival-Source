using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTwitch : MonoBehaviour
{
    //we need to set light min, max intensity and min and max flicker time. 
    //then we will randomly use a value from this range to make the flicker seem natural.

    public Light spotlight;
    public float minFlkrTime = 0.07f;
    public float maxFlkrTime = 0.1f;
    public float minBrightness = 0.5f;
    public float maxBrightness = 2.3f;
    public float nextFlkrTime;
    // Start is called before the first frame update
    void Start()
    {//this if statement asssigns the spotlight from GameObject if its already not manually assigned.
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();

        }
        setRndmFlkrTime();
        
    }


    // Update is called once per frame
    void Update()
    { //this decreases the next flicker time as time goes on.
        nextFlkrTime -= Time.deltaTime;

        if (nextFlkrTime < 0)
        {
            //changes light brightness
            spotlight.intensity = Random.Range(minBrightness, maxBrightness);
            setRndmFlkrTime();
        }
        
    }

    void setRndmFlkrTime()
    {
        //this sets flicker times using range function.
        nextFlkrTime = Random.Range(maxFlkrTime, minFlkrTime);
    }
}

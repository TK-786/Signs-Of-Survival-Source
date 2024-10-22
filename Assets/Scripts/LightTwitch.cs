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
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();

        }
        setRndmFlkrTime();
        
    }


    // Update is called once per frame
    void Update()
    {
        nextFlkrTime -= Time.deltaTime;

        if (nextFlkrTime < 0)
        {
            spotlight.intensity = Random.Range(minBrightness, maxBrightness);
            setRndmFlkrTime();
        }
        
    }

    void setRndmFlkrTime()
    {
        nextFlkrTime = Random.Range(maxFlkrTime, minFlkrTime);
    }
}

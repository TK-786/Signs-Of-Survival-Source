using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;

    void Update()
    {
        checkActive();
    }

    void checkActive()
    {
        int active = PlayerPrefs.GetInt("VisualSoundEffects", 1);
        Debug.Log(active);
        if (active == 1)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }
}

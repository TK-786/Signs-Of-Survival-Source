using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUIScript : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

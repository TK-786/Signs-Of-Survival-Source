using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestruction : MonoBehaviour
{
    public float destroyTime = 5f;

    void Start()
    {
        Destroy(gameObject, destroyTime); 
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); 
    }
}

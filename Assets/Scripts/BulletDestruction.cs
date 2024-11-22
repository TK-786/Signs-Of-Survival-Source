using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestruction : MonoBehaviour
{
    public float destroyTime = 5f;

    void Start()
    {
        Destroy(gameObject, destroyTime); // Automatically destroy the bullet after a set time
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // Destroy the bullet upon collision with any object
    }
}

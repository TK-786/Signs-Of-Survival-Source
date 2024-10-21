using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position ;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = pos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.Rotate (new Vector3 (15 ,30 ,45) * Time.deltaTime ) ;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
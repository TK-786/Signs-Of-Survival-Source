using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GunMechanics : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 100000000f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;
    private Item item;

    void Start()
    {
        item = GetComponent<Item>();
    }

    void Update()
    {
        if (item != null && item.isHeld && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        Destroy(bullet, 10f);
        Debug.Log("Shot fired!");
    }
}

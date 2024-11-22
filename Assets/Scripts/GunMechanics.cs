using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMechanics : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
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

        Destroy(bullet, 5f);
        Debug.Log("Shot fired!");
    }
}

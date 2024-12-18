using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMechanics : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 100f;
    public float fireRate = 0.5f;

    public int maxAmmo = 10; 
    private int currentAmmo; 

    private float nextFireTime = 0f;
    private Item item;

    private bool isGunUsable = true; 

    void Start()
    {
        item = GetComponent<Item>();
        currentAmmo = maxAmmo; 
    }

    void Update()
    {
        if (!isGunUsable) return; 

        if (item != null && item.isHeld && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Debug.Log("Out of ammo! The gun is now useless.");
                isGunUsable = false; 
            }
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        currentAmmo--; 
        Debug.Log("Shot fired! Ammo left: " + currentAmmo);

        Destroy(bullet, 10f);
    }
}

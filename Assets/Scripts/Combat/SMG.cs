using System.Collections;
using UnityEngine;

public class SMGMechanics : MonoBehaviour, IUsable
{
    public GameObject smgBulletPrefab;  // The bullet prefab
    public Transform firePoint;     // The point from which bullets are fired
    public float bulletSpeed = 100f;  // Speed of the bullets
    public float fireRate = 0.1f;   // Very fast fire rate for SMG
    public int maxAmmo = 30;        // High ammo capacity
    private int currentAmmo;
    private float nextFireTime = 0f;
    private Item item;

    void Start()
    {
        currentAmmo = maxAmmo;  // Initialize ammo
        item = GetComponent<Item>();
    }

    public void OnUse() {
        if (item != null && item.isHeld && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Debug.Log("Out of ammo! SMG cannot be used anymore.");
            }
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;  // Set time for next shot
        currentAmmo--;

        // Create and launch the bullet
        GameObject bullet = Instantiate(smgBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        Debug.Log("SMG shot fired!");

        Destroy(bullet, 5f);  // Destroy the bullet after 5 seconds
    }
}

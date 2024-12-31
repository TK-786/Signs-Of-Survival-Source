using System.Collections;
using UnityEngine;

public class SniperMechanics : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 150f; // Faster than the gun bullet
    public float fireRate = 1.5f; // Slower fire rate
    public int maxAmmo = 5; // Limited ammo
    public float zoomFOV = 20f; // Field of view when zoomed in
    public float zoomSpeed = 5f; // Speed at which zoom happens
    public float defaultFOV = 60f; // Default field of view

    private int currentAmmo;
    private float nextFireTime = 0f;
    private Item item;
    private Camera playerCamera; // Reference to the player's camera
    private bool isZoomed = false; // Tracks whether zoom is active

    void Start()
    {
        currentAmmo = maxAmmo;
        item = GetComponent<Item>();

        // Get the player's camera
        playerCamera = Camera.main;
        if (playerCamera != null)
        {
            defaultFOV = playerCamera.fieldOfView;
        }
    }

    void Update()
    {
        // Handle shooting
        if (item != null && item.isHeld && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                Debug.Log("Out of ammo! Sniper cannot be used anymore.");
            }
        }

        // Handle zoom with the Z key
        if (item != null && item.isHeld && Input.GetKeyDown(KeyCode.Z)) // Press Z to toggle zoom
        {
            isZoomed = !isZoomed; // Toggle zoom state
        }

        if (playerCamera != null)
        {
            float targetFOV = isZoomed ? zoomFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        }
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        Debug.Log("Sniper shot fired!");

        Destroy(bullet, 15f); // Destroy the bullet after 15 seconds
    }
}

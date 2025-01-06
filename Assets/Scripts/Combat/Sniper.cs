using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class SniperMechanics : MonoBehaviour, IUsable
{
    public GameObject sniperBulletPrefab;
    private Transform firePoint;
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
    public GameObject player;
    public Camera mainCamera;

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
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");

            mainCamera = player.GetComponentInChildren<Camera>();

            firePoint = mainCamera.transform;
        }
        // Handle zoom with the Z key
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            isZoomed = !isZoomed; // Toggle zoom state
        }

        // Smoothly transition the camera field of view
        if (playerCamera != null)
        {
            float targetFOV = isZoomed ? zoomFOV : defaultFOV;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
        }

        // Check for shooting
        if (Mouse.current.leftButton.wasPressedThisFrame && item != null && item.isHeld)
        {
            OnUse(); // Call the OnUse() method to handle shooting
        }
    }
    public void OnUse()
    {
        // Handle shooting
        if (item != null && item.isHeld && Time.time >= nextFireTime)
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
        if (item != null && item.isHeld) // Press Z to toggle zoom
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

        GameObject bullet = Instantiate(sniperBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        Debug.Log("Sniper shot fired!");

        Destroy(bullet, 15f); // Destroy the bullet after 15 seconds
    }

}
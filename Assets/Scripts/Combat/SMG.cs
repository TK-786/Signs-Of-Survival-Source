using System.Collections;
using UnityEngine;

public class SMGMechanics : MonoBehaviour, IUsable
{
    public GameObject smgBulletPrefab;
    private Transform firePoint;
    public float bulletSpeed = 100f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    private int currentAmmo;
    private float nextFireTime = 0f;
    private Item item;
    public GameObject player;
    public Camera mainCamera;

    void Start()
    {
        currentAmmo = maxAmmo;
        item = GetComponent<Item>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");

            mainCamera = player.GetComponentInChildren<Camera>();

            firePoint = mainCamera.transform;
        }
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

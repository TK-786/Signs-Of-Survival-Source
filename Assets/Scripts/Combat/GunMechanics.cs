using UnityEngine;
using UnityEngine.InputSystem;

public class GunMechanics : MonoBehaviour, IUsable, IWeapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    private float nextFireTime = 0f;
    private Item item;
    private int currentAmmo;

    void Start()
    {
        item = GetComponent<Item>();
    }

    public void OnUse()
    {
        if (item.isHeld && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        Destroy(bullet, 5f);
        Debug.Log("Shot fired!");
    }
}
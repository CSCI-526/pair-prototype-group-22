using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Gun1 : MonoBehaviour
{
    public GameObject bulletPrefab;  // Assign your bullet prefab in the Inspector
    public Transform firePoint;      // The position where bullets spawn
    public float bulletSpeed = 20f;  // Speed of the bullet
    public float fireRate = 2f;    // Fire rate (bullets per second)
    public string owner_tag;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, fireRate);
        firePoint = gameObject.transform.Find("Cylinder");
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Bullet Prefab or Fire Point is not assigned!");
            return;
        }

        // Instantiate the bullet at the firePoint position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().owner_tag = owner_tag;

        // Get the Rigidbody component and apply force to move the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.up * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab needs a Rigidbody component!");
        }
    }
}

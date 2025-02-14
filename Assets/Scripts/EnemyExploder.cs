using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExploder : MonoBehaviour
{
    public Transform playerShip;  // Assign the Player Ship in Inspector
    public float detectionRadius = 7f;  // Explosion trigger radius

    public GameObject explosionEffect; // Assign an explosion effect prefab
    public GameObject playerObject; // Reference to the Player

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerShip.position);

        if (distance < detectionRadius)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy the player ship
        Destroy(playerObject);

        // Destroy the enemy ship itself
        Destroy(gameObject);
    }

    // Call this function when the enemy gets hit by a player's bullet
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

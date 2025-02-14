using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExploder : MonoBehaviour
{
    public Transform playerShip;  // Assign the Player Ship in Inspector
    public float detectionRadius = 15.0f;  // Follow trigger radius

    public GameObject explosionEffect; // Assign an explosion effect prefab
    public GameObject playerObject; // Reference to the Player

    public float lookSpeed = 0.3f;
    public float moveSpeed = 0.9f;

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
            Vector3 targetDirection = playerShip.position - transform.position;
            // The step size is equal to speed times frame time.
            float singleStep = lookSpeed * Time.deltaTime;

            // find cross of x-axis with where the target is
            Vector3 rotationAxis = Vector3.Cross(transform.right, targetDirection).normalized;
            rotationAxis.x = 0.0f;
            rotationAxis.z = 0.0f;

            transform.rotation = Quaternion.AngleAxis(singleStep * Mathf.Rad2Deg, rotationAxis) * transform.rotation;

            // move towards the x-axis at constant speed
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
    }

    void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // apply knockBack to player here
        Vector3 targetDirection = playerShip.position - transform.position;
        playerObject.GetComponent<PlayerMovement>().applyKnockBack(-1 * targetDirection.normalized);

        // Destroy the enemy ship itself
        Destroy(gameObject);
    }

    // Call this function when the enemy gets hit by a player's bullet
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // keep player level to ground
        if (collisionInfo.gameObject.tag == "Player")
        {
            Explode();
        }

    }
}

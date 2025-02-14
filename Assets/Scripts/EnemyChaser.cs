using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    public Transform playerShip;   // Assign the Player Ship in Inspector
    public float detectionRadius = 10f;  // Radius to detect the player
    public float moveSpeed = 5f;   // Speed of enemy movement

    private bool isChasing = false; // Flag to check if chasing

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
            isChasing = true; // Start chasing
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerShip.position, moveSpeed * Time.deltaTime);
    }

    // Call this function when the enemy gets hit by a player's bullet
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

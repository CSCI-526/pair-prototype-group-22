using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    public Transform playerShip;   // Assign the Player Ship in Inspector
    public float detectionRadius = 15.0f;  // Radius to detect the player

    public float lookSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShip != null)
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
            }
        }
    }

    // Call this function when the enemy gets hit by a player's bullet
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

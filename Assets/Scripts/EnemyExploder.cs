using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyExploder : MonoBehaviour
{
    public Transform playerShip;  // Assign the Player Ship in Inspector
    public float detectionRadius = 15.0f;  // Follow trigger radius

    public float knockBackDuration = 0.75f;
    
    public GameObject explosionEffect; // Assign an explosion effect prefab
    public GameObject playerObject; // Reference to the Player

    public float lookSpeed = 0.3f;
    public float moveSpeed = 0.9f;
    
    private bool isKnockBack;
    private float knockBackFactor = 10.0f;
    private Rigidbody rb;
    private float initYPos;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        playerShip = playerObject.transform;
        isKnockBack = false;
        rb = GetComponent<Rigidbody>();
        initYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = initYPos;
        transform.position = pos;
        
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
    
    void Explode2(GameObject bullet)
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        knockBackFactor = 10.0f * Mathf.Sqrt(bullet.GetComponent<Rigidbody>().mass / 200f);

        // apply knockBack to player here
        Vector3 targetDirection = transform.position - bullet.transform.position;
        applyKnockBack(-1 * targetDirection.normalized);

        // Destroy the enemy ship itself
        Destroy(bullet);
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
        if (collisionInfo.gameObject.tag == "bullet")
        {
            GameObject bullet = collisionInfo.gameObject;
            if (bullet.GetComponent<Bullet>())
            {
                if (bullet.GetComponent<Bullet>().owner_tag == "Player")
                {
                    Explode2(bullet);
                }
            } else if (bullet.GetComponent<BulletLarge>())
            {
                if (bullet.GetComponent<BulletLarge>().owner_tag == "Player")
                {
                    Explode2(bullet);
                }
            }
        }
    }
    
    public void applyKnockBack(Vector3 direction)
    {
        if (!isKnockBack)
        {
            StartCoroutine(KnockBackOverTime(direction));
        }
    }

    private IEnumerator KnockBackOverTime(Vector3 direction)
    {
        isKnockBack = true;
        float elapsedTime = 0f;

        while (elapsedTime < knockBackDuration)
        {
            transform.Translate(direction * 1.0f * knockBackFactor * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.velocity = new Vector3(0, 0, 0);

        isKnockBack = false;
    }
}

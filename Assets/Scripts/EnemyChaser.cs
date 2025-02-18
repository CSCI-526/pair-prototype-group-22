using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{

    public Transform playerShip;   // Assign the Player Ship in Inspector
    public float detectionRadius = 15.0f;  // Radius to detect the player
    private GameObject playerObj;
    private float initYPos;
    public float knockBackDuration = 0.5f;
    public float lookSpeed = 0.3f;
    public GameObject explosionEffect;
    
    private bool isKnockBack;
    private bool inRock;
    private float knockBackFactor = 1.0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        playerShip = playerObj.transform;
        initYPos = transform.position.y;
        rb = GetComponent<Rigidbody>();
        inRock = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = initYPos;
        transform.position = pos;
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

        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
    }
    
    void OnCollisionEnter(Collision collisionInfo)
    {
        // keep player level to ground
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

        if (collisionInfo.gameObject.tag == "Course")
        {
            inRock = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Course")
        {
            inRock = false;
        }
    }

    void Explode2(GameObject bullet)
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        knockBackFactor = knockBackFactor * Mathf.Sqrt(bullet.GetComponent<Rigidbody>().mass / 200f);

        // apply knockBack to enemy here
        Vector3 targetDirection = bullet.GetComponent<Rigidbody>().velocity; ;
        targetDirection.y = 0.0f;
        applyKnockBack(targetDirection.normalized);

        // Destroy the bullet
        Destroy(bullet);
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
            transform.Translate(direction * 2.0f * knockBackFactor * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            elapsedTime += Time.deltaTime;
            if (inRock)
            {
                break;
            }
            yield return null;
        }
        rb.velocity = new Vector3(0, 0, 0);

        isKnockBack = false;
    }

    // Call this function when the enemy gets hit by a player's bullet
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

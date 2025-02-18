using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedConstant = 3.0f;
    public float maxRotationSpeed = 20.0f;
    public float rotationAcceleration = 0.01f;
    public float rotationDecceleration = 0.01f;
    public float knockBackDuration = 1.0f;
    public float knockBackFactor = 1.0f;
    public GameObject sailFront;
    public GameObject sailBack;
    public GameObject explosionEffect;
    
    private float currentRotationSpeed;
    private float horizontalInput;
    private float verticalInput;
    private bool isKnockBack;
    private bool inRock;
    private Rigidbody rb;
    const float MAX_SAIL_LENGTH = 1.0f;
    const float fixedYLevel = 1.65f;
    private float sailLength = 0.5f; // default it to half
    private float bulletNerf = 0.2f;

    void Start()
    {
        // intialize sails to half
        sailFront.transform.localScale = new Vector3(0.05f, 0.1f, 0.1f);
        sailBack.transform.localScale = new Vector3(0.05f, 0.1f, 0.1f);

        // intialize rigidbody
        rb = GetComponent<Rigidbody>();

        // intialize other functions
        isKnockBack = false;
        inRock = false;
    }

    private void OnEnable()
    {
        // adjust parameter based on mass
        rb = GetComponent<Rigidbody>();
        knockBackFactor = knockBackFactor * 2000 / rb.mass;
        knockBackDuration = knockBackDuration * 1250 / rb.mass;
        speedConstant = speedConstant / Mathf.Pow(rb.mass / 1000, 1.02f);
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // apply horizontal Speed here based on sailLength
        transform.Translate(Vector3.right * Time.deltaTime * sailLength * speedConstant);
        // keep the player transform at a set level, so they can't jump over stuff
        Vector3 pos = transform.position;
        pos.y = fixedYLevel;
        transform.position = pos;

        if (verticalInput != 0)
        {
            // movement down button
            sailLength = Mathf.Clamp(sailLength + -0.25f * verticalInput * Time.deltaTime, 0.0f, MAX_SAIL_LENGTH);
            // scale sail to sailLength
            sailFront.transform.localScale = new Vector3(sailLength * 0.1f, 0.1f, 0.1f);
            sailBack.transform.localScale = new Vector3(sailLength * 0.1f, 0.1f, 0.1f);
        }

        // logic for rotation here
        if (horizontalInput != 0)
        {
            // Increase rotation speed while horizontal Input is applied
            currentRotationSpeed += horizontalInput * rotationAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);
        }
        else
        {
            if (currentRotationSpeed > 0)
            {
                // when key is let go, gradually reset the rotation to 0
                currentRotationSpeed -= rotationDecceleration * Time.deltaTime;
                currentRotationSpeed = Mathf.Max(currentRotationSpeed, 0);
            }
            else if (currentRotationSpeed < 0)
            {
                // same but for the opposite direction
                currentRotationSpeed += rotationDecceleration * Time.deltaTime;
                currentRotationSpeed = Mathf.Min(currentRotationSpeed, 0);
            }
        }

        // apply rotation and keep other axis from moving
        transform.Rotate(0.0f, currentRotationSpeed, 0.0f, Space.World);
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        // keep player level to ground
        if(collisionInfo.gameObject.tag == "Course" || collisionInfo.gameObject.tag == "enemy")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }
        if (collisionInfo.gameObject.tag == "Course")
        {
            Debug.Log("Im stuck");
            inRock = true;
            // apply force away from rock so user is not clipped
            Vector3 awayDirection = collisionInfo.gameObject.GetComponent<Transform>().position - transform.position;
            awayDirection.y = 0.0f;
            rb.AddForce(awayDirection * 100.0f * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // keep player level to ground
        if (collisionInfo.gameObject.tag == "Course" || collisionInfo.gameObject.tag == "enemy")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }
        if (collisionInfo.gameObject.tag == "Course")
        {
            inRock = true;
        }
        if (collisionInfo.gameObject.tag == "bullet")
        {
            GameObject bullet = collisionInfo.gameObject;
            if (bullet.GetComponent<Bullet>())
            {
                if (bullet.GetComponent<Bullet>().owner_tag == "enemy")
                {
                    bulletKnockBack(bullet);
                }
            } else if (bullet.GetComponent<BulletLarge>())
            {
                if (bullet.GetComponent<BulletLarge>().owner_tag == "enemy")
                {
                    bulletKnockBack(bullet);
                }
            }
        }
    }

    void bulletKnockBack(GameObject bullet)
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        //knockBackFactor = 10.0f * Mathf.Sqrt(bullet.GetComponent<Rigidbody>().mass / 200f);

        // apply knockBack to player here
        //Vector3 targetDirection = transform.position - bullet.transform.position;
        Vector3 targetDirection = bullet.GetComponent<Rigidbody>().velocity;
        targetDirection.y = 0.0f;
        Debug.Log(targetDirection.normalized);
        applyKnockBack(targetDirection.normalized * bulletNerf);

        // Destroy the enemy ship itself
        Destroy(bullet);
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        // keep player level to ground
        if (collisionInfo.gameObject.tag == "Course" || collisionInfo.gameObject.tag == "enemy")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }
        if (collisionInfo.gameObject.tag == "Course")
        {
            inRock = false;
        }

        if (rb)
        {
            rb.velocity = new Vector3(0, 0, 0);
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
            transform.Translate(direction * speedConstant * knockBackFactor * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            if (inRock)
            {
                break;
            }
            yield return null;
        }
        rb.velocity = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);

        isKnockBack = false;
    }
}

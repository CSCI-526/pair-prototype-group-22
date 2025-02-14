using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedConstant = 1.0f;
    public float maxRotationSpeed = 0.05f;
    public float rotationAcceleration = 0.005f;
    public float rotationDecceleration = 0.01f;
    public float knockBackDuration = 0.75f;
    public GameObject sailFront;
    public GameObject sailBack;

    private float currentRotationSpeed;
    private float horizontalInput;
    private float verticalInput;
    private bool isKnockBack;
    private bool inRock;
    private Rigidbody rb;
    const float MAX_SAIL_LENGTH = 1.0f;
    const float fixedYLevel = 1.65f;
    private float sailLength = 0.5f; // default it to half
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
            currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -maxRotationSpeed, maxRotationSpeed);
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
        if(collisionInfo.gameObject.tag == "Course")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            inRock = true;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // keep player level to ground
        if (collisionInfo.gameObject.tag == "Course")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            inRock = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        // keep player level to ground
        if (collisionInfo.gameObject.tag == "Course")
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            inRock = false;
        }
        rb.velocity = new Vector3(0, 0, 0);
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
            transform.Translate(direction * speedConstant * 10.0f * Time.deltaTime);
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
}

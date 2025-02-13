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
    public GameObject sailFront;
    public GameObject sailBack;

    private float currentRotationSpeed;
    private float horizontalInput;
    private float verticalInput;
    const float MAX_SAIL_LENGTH = 1.0f;
    const float fixedYLevel = 1.65f;
    private float sailLength = 0.5f; // default it to half
    void Start()
    {
        
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
        transform.Rotate(0.0f, currentRotationSpeed, 0.0f);
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust to change how fast the player moves

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

// Optionally, to keep the player from tipping over:
// rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (BuildingSystem.Instance.IsPlacing) return;
// Get WASD/arrow input
        float h = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxis("Vertical"); // W/S or Up/Down

// Build a velocity vector based on input
        Vector3 velocity = new Vector3(h, 0f, v) * moveSpeed;

// Assign it to the Rigidbody's velocity
        rb.velocity = velocity;
    }
}

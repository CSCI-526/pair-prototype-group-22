using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;       // How fast we rotate around the target
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;          // How fast we zoom in/out
    public float minDistance = 2f;         // Minimum distance from the target (zoom in limit)
    public float maxDistance = 50f;        // Maximum distance from the target (zoom out limit)

    [Header("Panning Settings")]
    public float panSpeed = 0.5f;          // How fast we pan the camera

    // We’ll use a "pivot" or "target" point around which we’ll rotate the camera.
    // If you don’t already have a central object or pivot, you could create an empty
    // GameObject in your scene and place this as the target to orbit around.
    public Transform pivot;                // The point in the scene around which we rotate
    
    private Vector3 lastMousePosition;     // Track mouse position for panning
    
    void Update()
    {
        // --- Rotation (Right Mouse Button) ---
        if (Input.GetMouseButton(1))  // Right-click drag
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate around the pivot’s Y-axis (horizontal)
            transform.RotateAround(pivot.position, Vector3.up, mouseX * rotationSpeed);
            
            // Rotate around the pivot’s X-axis (vertical)
            // Negative mouseY so that dragging "up" rotates the camera down
            transform.RotateAround(pivot.position, transform.right, -mouseY * rotationSpeed);
        }

        // --- Zoom (Mouse Scroll Wheel) ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Distance from camera to pivot
            float currentDistance = Vector3.Distance(transform.position, pivot.position);

            // Calculate the desired zoom distance
            float distanceAfterZoom = currentDistance - scroll * zoomSpeed;

            // Clamp the distance so we don’t go too close or too far
            distanceAfterZoom = Mathf.Clamp(distanceAfterZoom, minDistance, maxDistance);

            // Move the camera along its forward vector
            Vector3 direction = (transform.position - pivot.position).normalized;
            transform.position = pivot.position + direction * distanceAfterZoom;
        }

        // --- Pan (Middle Mouse Button) ---
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))  // Middle-click drag
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // Move (pan) the pivot and the camera together
            // We use transform.right and transform.up to move relative to the camera's orientation
            pivot.Translate(-delta.x * panSpeed * Time.deltaTime * transform.right, Space.World);
            pivot.Translate(-delta.y * panSpeed * Time.deltaTime * transform.up, Space.World);

            // The camera’s position will also effectively move along with the pivot
            // if we orbit around the pivot.
            // Alternatively, you could move the camera directly.
            transform.Translate(-delta.x * panSpeed * Time.deltaTime * transform.right, Space.World);
            transform.Translate(-delta.y * panSpeed * Time.deltaTime * transform.up, Space.World);
        }
    }
}

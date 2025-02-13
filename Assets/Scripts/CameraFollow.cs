using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraRotationSpeed = 0.1f;
    private Vector3 offset = new Vector3(5.0f, 1.5f, 5.0f);
    private Vector3 rotationOffset = new Vector3(20.0f, 90.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion playerRotation = player.rotation;

        // point camera at player with a given circular track
        float angleToCenter = Mathf.Deg2Rad * (playerRotation.eulerAngles.y - 90.0f);
        Vector3 adjustedOffset = new Vector3(offset.x * Mathf.Sin(angleToCenter), 
            offset.y, offset.z * Mathf.Cos(angleToCenter));
        Vector3 cameraPosition = player.position + adjustedOffset;
        transform.position = cameraPosition;

        // get camera rotation correct with intial offset
        playerRotation.eulerAngles += rotationOffset;
        transform.rotation = playerRotation;
    }
}

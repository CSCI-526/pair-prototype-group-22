using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    // If you want, you can store a surface normal or do something fancy:
    // public Vector3 surfaceNormal = Vector3.up;

    private void OnTriggerEnter(Collider other)
    {
        // 1) Make sure the BuildingSystem exists and is in "placing mode"
        if (BuildingSystem.Instance == null) return;
        if (!BuildingSystem.Instance.IsPlacing) return;

        // 2) Check if the object entering the trigger is the "preview" object
        // (assuming you tag your preview object as "Preview")
        if (other.CompareTag("Preview"))
        {
            // 3) Tell the building system this node is now hovered
            BuildingSystem.Instance.OnNodeHoverEnter(this);
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (BuildingSystem.Instance == null) return;
    //     if (!BuildingSystem.Instance.IsPlacing) return;
    //
    //     // If the "preview" object left the node
    //     if (other.CompareTag("Preview"))
    //     {
    //         BuildingSystem.Instance.OnNodeHoverExit(this);
    //     }
    // }

    public void OnMouseClick()
    {
        // Debug.Log("Mouse Down");
        // If we click on this node, attempt to place the object
        if (BuildingSystem.Instance == null) return;
        if (!BuildingSystem.Instance.IsPlacing) return;
        // Debug.Log("Mouse Down1");

        BuildingSystem.Instance.PlaceObjectAtNode(this);
    }
}

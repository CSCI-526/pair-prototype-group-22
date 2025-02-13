using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*if (BuildingSystem.Instance) {
            player = BuildingSystem.Instance.player;
        }
        else
        {
            Debug.Log("Player not found");
        }*/
    }

    public void OnAttachUpdate()
    {
        Rigidbody rb = BuildingSystem.Instance.player.GetComponent<Rigidbody>();
        // Debug.Log("here");
        if (gameObject.CompareTag("build_part1"))
        {
            rb.mass += 10;
            rb.drag += 10;
            rb.angularDrag += 10;
        } else if (gameObject.CompareTag("build_part2"))
        {
            rb.mass += 1;
            rb.drag /= 2;
            rb.angularDrag /= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

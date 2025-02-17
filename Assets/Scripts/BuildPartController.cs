using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPartController : MonoBehaviour
{
    public float size = 0.25f;
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
            rb.mass += 500;
            rb.drag += 100;
            rb.angularDrag += 1000;
        } else if (gameObject.CompareTag("build_part2"))
        {
            rb.mass += 150;
            rb.drag /= 2;
            //rb.angularDrag /= 2; Angular mass needs to be kept hight to reduce unwanted rotaiton
        } else if (gameObject.CompareTag("build_part3"))
        {
            rb.mass += 100;
            rb.angularDrag += 100;
        }
        else
        {
            rb.mass += 250;
            rb.angularDrag += 1000;
            rb.drag += 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

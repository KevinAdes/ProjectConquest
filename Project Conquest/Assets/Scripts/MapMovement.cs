using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float speed;
    public Camera mainCamera;
    Vector3 move = new Vector3(0, 0, 0);
    Vector3 horizon = new Vector3(0, 0, 0);
    Vector3 verizon = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
        Movement();
    }

    private void Movement()
    {

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            horizon = new Vector3(speed, 0);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            horizon = new Vector3(-speed, 0);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            horizon = new Vector3(0, 0);
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            verizon = new Vector3(0, speed);
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            verizon = new Vector3(0, -speed);
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            verizon = new Vector3(0, 0);
        }

        move = (horizon + verizon).normalized * speed;
        transform.position += move;
    }
}

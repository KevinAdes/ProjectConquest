using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMovement : MonoBehaviour
{
    public float speed;

    string target = null;

    Camera mainCamera;
    LevelData temp;
    GameManager manager;

    Vector2 move = new Vector2(0, 0);
    Vector2 horizon = new Vector2(0, 0);
    Vector2 verizon = new Vector2(0, 0);


    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
            transform.position = manager.mapulaTransform;
            manager.Mapula = this;
        }
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera = FindObjectOfType<Camera>();
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
        Movement();
        Select();
        
    }

    private void Select()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            if (target != null && Time.timeScale > 0)
            {
                manager.LoadLevel(target);
            }

        }
    }

    private void Movement()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(manager.table.Levels.Count);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            horizon = new Vector2(speed, 0);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            horizon = new Vector2(-speed, 0);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            horizon = new Vector2(0, 0);
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            verizon = new Vector2(0, speed);
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            verizon = new Vector2(0, -speed);
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            verizon = new Vector2(0, 0);
        }

        move = (horizon + verizon).normalized * speed;
        GetComponent<Rigidbody2D>().velocity = move;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        { 
            target = collision.gameObject.GetComponent<Level>().ID;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            target = null;
        }
    }

}

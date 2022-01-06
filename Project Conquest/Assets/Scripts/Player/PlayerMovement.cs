using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxHealth;
    public float damage;
    public float defense;
    public float speed;
    public float jump;
    public float attackModifier;

    public float health;
    public float speedCap;
    public float acceleration;

    [Header("Components")]
    public Rigidbody2D body;
    public Animator animator;
    public Collider2D collider2;

    [Header("Extras")]
    public LayerMask enemies;

    Vector2 velocity;

    bool isGrounded;
    bool attack;
    bool drink;
    //functionally a bool that alternates between -1 and 1.
    int right = 1;

    public float speedCache;
    public float speedCapCache;
    float scaleCache;

    string STATE;

    HumanController target;
    GameManager manager;
    Camera mainCamera;
    PauseControl pauseControl;

    //^^^^VARIABLES^^^^
    //############################################################################
    //vvvvSCRIPTvvvv

    public void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        StartCoroutine(SpawnPoint());
        pauseControl = FindObjectOfType<PauseControl>();
        velocity = new Vector2(0, 0);
        body.velocity = velocity;
        STATE = "Default";
        health = maxHealth;
        speedCache = speed;
        speedCapCache = speedCap;
        scaleCache = transform.localScale.x;
    }
    IEnumerator SpawnPoint()
    {
        yield return new WaitForEndOfFrame();

        transform.position = manager.playerLevelTransform;
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z - 5);

        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.GetMask("Ground"));
        
        switch (STATE)
        {
            case "Default":
                Run();
                Jumping();
                Crouch();
                Direction();
                break;
            case "Drink":
                Run();
                Jumping();
                Drink();
                Direction();
                Crouch();
                break;
            case "Drinking":
                break;
        }
        

    }

    //Control Functions
    //

    private void Direction()
    {
        if (attack == false)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(scaleCache * -1, transform.localScale.y, transform.localScale.z);
                right = -1;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(scaleCache, transform.localScale.y, transform.localScale.z);
                right = 1;
            }
        }
    }

    private void Jumping()
    {
        if (Input.GetAxis("Jump") != 0 && isGrounded && animator.GetBool("Crouch") == false)
        {
            velocity.x = body.velocity.x;
            velocity.y = jump * Input.GetAxis("Jump");
            body.velocity = velocity;
        }
        if (isGrounded == false && Input.GetAxis("Vertical") < -0.75 && body.velocity.y < 3)
        {
            velocity.x = body.velocity.x * .3f;
            velocity.y = -jump * 2;
            body.velocity = velocity;
        }
    }

    private void Run()
    {
        if (Input.GetAxis("Horizontal") != 0 && animator.GetBool("Crouch") == false)
        {
            if (speed < speedCap)
            {
                speed += (speedCache / speedCapCache) * acceleration;
            }
            velocity.x = speed * Input.GetAxis("Horizontal");
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }

        if (Input.GetAxis("Horizontal") == 0 && animator.GetBool("Crouch") == false)
        {
            speedCap = speedCapCache;
            speed = (speed + speedCache) / 2;
            velocity.x = speed * Input.GetAxis("Horizontal");
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }
    }

    private void Drink()
    {
        if (Input.GetAxis("Fire1") != 0 && drink == false)
        {
            STATE = "Drinking";
            drink = true;
            //Gain extra exp from victim
            animator.SetTrigger("Drink");
            AddXP(target.expYield);
            target.animator.SetTrigger("Eaten");
        }

    }

    private void Crouch()
    {
        if (Input.GetAxis("Vertical") < 0 && isGrounded == true && animator.GetBool("Crouch") == false)
        {
            animator.SetBool("Crouch", true);
        }
        if (Input.GetAxis("Vertical") >= -.5)
        {
            animator.SetBool("Crouch", false);
        }
    }

    //Animation Functions
    //

    private void DrinkDone()
    {
        STATE = "Default";
        drink = false;
    }

    public void StateSwitcher(string State)
    {
        STATE = State;
    }
    

    public void AddXP(int gains)
    {
        pauseControl.playerData.blood += gains;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            manager.LoadLevel("Map");
        }

        if (collision.gameObject.layer == 10)
        {
            if(collision.transform.parent.gameObject.GetComponent<HumanController>() != null)
            {
                print("and i am ready to drink it");
                target = collision.transform.parent.gameObject.GetComponent<HumanController>();
                target.TakeDamage(target.health);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            target = null;
            StateSwitcher("Default");
        }
    }
}

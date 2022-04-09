using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed;
    float jump;

    float speedCap;
    float acceleration;

    [Header("Components")]
    [SerializeField]
    Rigidbody2D body;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Collider2D collider2;

    [Header("Extras")]
    public LayerMask enemies;
    public LayerMask ground;

    Vector2 velocity;

    bool isGrounded;
    bool attack;

    float speedCache;
    float speedCapCache;
    float scaleCache;

    states STATE;

    Entity target;
    GameManager manager;
    Camera mainCamera;

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
        velocity = new Vector2(0, 0);
        body.velocity = velocity;
        STATE = states.DEFAULT;
        speedCache = speed;
        speedCapCache = speedCap;
        scaleCache = transform.localScale.x;
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z - 5);

        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), ground);
        
        switch (STATE)
        {
            case states.DEFAULT:
                Run();
                Jumping();
                Crouch();
                Direction();
                break;
            case states.SCAVENGING:
                Freeze();
                break;
            case states.DIALOGUE:
                Freeze();
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
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(scaleCache, transform.localScale.y, transform.localScale.z);
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
    private void Freeze()
    {
        velocity = new Vector2(0, 0);
        body.velocity = velocity;
    }

    public void StateSwitcher(states State)
    {
        STATE = State;
    }

    //Setters and Getters

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void SetSpeed(float f)
    {
        speed = f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetJump(float f)
    {
        jump = f;
    }

    public void SetSpeedCap(float f)
    {
        speedCap = f;
    }

    public void SetAcceleration(float f)
    {
        acceleration = f;
    }

    public void SetSpeedCache(float f)
    {
        speedCache = f;
    }

    public void SetSpeedCapCache(float f)
    {
        speedCapCache = f;
    }

}

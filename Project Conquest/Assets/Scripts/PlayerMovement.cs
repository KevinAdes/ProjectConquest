using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Stats")]
    public float health;
    public float damage;
    public float defense;
    public float Speed;
    public float Jump;

    [Header("Calculation Variables")]
    public float SpeedCap;
    public float Acceleration;

    [Header("Components")]
    public Rigidbody2D Body;
    public Camera MainCamera;
    public Animator animator;

    Vector2 velocity = new Vector2(0, 0);

    bool isGrounded;
    bool boost;
    bool moving;
    bool attack;

    float SpeedCache;
    float SpeedCapCache;
    float JumpCache;
    float ScaleCache;

    //functionally a bool that alternates between -1 and 1.
    int right = 1;

    void Start()
    { 
        SpeedCache = Speed;
        SpeedCapCache = SpeedCap;
        JumpCache = Jump;
        ScaleCache = transform.localScale.x;
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.GetMask("Ground"));
        MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z -5);
        Run();
        Jumping();
        Attack();
        Crouch();
        Direction();
    }

    private void Direction()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(ScaleCache * -1, transform.localScale.y, transform.localScale.z);
            right = -1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(ScaleCache, transform.localScale.y, transform.localScale.z);
            right = 1;
        }
    }

    private void Jumping()
    {
        if (Input.GetAxis("Jump") != 0 && isGrounded)
        {
            velocity.x = Body.velocity.x;
            velocity.y = Jump * Input.GetAxis("Jump");
            Body.velocity = velocity;
        }
        if (isGrounded == false && Input.GetAxis("Vertical") < -0.75 && Body.velocity.y < 3)
        {
            velocity.x = Body.velocity.x * .3f;
            velocity.y = -Jump * 2;
            Body.velocity = velocity;
        }
    }

    private void Run()
    {
        if (Input.GetAxis("Horizontal") != 0 && attack == false && animator.GetBool("Crouch") == false)
        {
            StartCoroutine(AmIStopped());
            StartCoroutine(RunTimer());
            if (Speed < SpeedCap)
            {
                Speed += (SpeedCache / SpeedCapCache) * Acceleration;
            }
            velocity.x = Speed * Input.GetAxis("Horizontal");
            velocity.y = Body.velocity.y;
            Body.velocity = velocity;
        }

        if (Input.GetAxis("Horizontal") == 0 && animator.GetBool("Crouch") == false)
        {
            moving = false;
            boost = false;
            Jump = JumpCache;
            SpeedCap = SpeedCapCache;
            Speed = (Speed + SpeedCache) / 2;
            velocity.x = Speed * Input.GetAxis("Horizontal");
            velocity.y = Body.velocity.y;
            Body.velocity = velocity;
        }
    }

    private void Attack()
    {

        if (Input.GetAxis("Fire1") != 0 && attack == false && animator.GetBool("Crouch") == true)
        {
            attack = true;
            animator.SetTrigger("Attack");
            velocity.x = SpeedCapCache * right;
            velocity.y = Body.velocity.y;
            Body.velocity = velocity;
        }

        if (Input.GetAxis("Fire1") != 0 && attack == false)
        {
            attack = true;
            animator.SetTrigger("Attack");
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

    private void AttackDone()
    {
        attack = false;
    }

    private void SlideDone()
    {
        velocity.x = 0;
        velocity.y = Body.velocity.y;
        Body.velocity = velocity;
    }

    IEnumerator RunTimer()
    {
        if (moving == true)
        {
            yield return new WaitForSeconds(10);

            if (boost == false && moving == true)
            {
                SpeedCap = SpeedCap * 1.25f;
                Jump = Jump * 1.5f;
                boost = true;
            }
            if (boost == true)
            {
                yield break;
            }
        }
        else
        {
            Jump = JumpCache;
            SpeedCap = SpeedCapCache;
            yield break;
        }

    }
    IEnumerator AmIStopped()
    {
        float Gposition = GetComponent<Transform>().position.x;
        yield return new WaitForSeconds(2);
        if(Gposition == GetComponent<Transform>().position.x)
        {
            moving = false;
        } else
        {
            moving = true;
        }

    }
}

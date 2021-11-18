using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D Body;
    public float Speed;
    public float Jump;
    public float SpeedCap;
    public float Acceleration;
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
        if (Input.GetAxis("Horizontal") != 0 && attack == false)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(ScaleCache * -1, transform.localScale.y, transform.localScale.z);
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(ScaleCache, transform.localScale.y, transform.localScale.z);
            }
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

        if (Input.GetAxis("Horizontal") == 0)
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
        
        if (Input.GetAxis("Fire1") != 0 && attack == false)
        {
            attack = true;
            animator.SetTrigger("Attack");

        }
        
    }

    private void Crouch()
    {
        return;
    }

    private void AttackDone()
    {
        attack = false;
    }
    IEnumerator RunTimer()
    {
        if (moving == true)
        {
            yield return new WaitForSeconds(10);

            if (boost == false && moving == true)
            {
                SpeedCap = SpeedCap * 2f;
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

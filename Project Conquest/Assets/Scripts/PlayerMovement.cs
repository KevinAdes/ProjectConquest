using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D Body;
    public float Speed;
    public float Jump;
    public float SpeedCap;
   
    Vector2 velocity = new Vector2(0, 0);

    bool isGrounded;
    bool boost;
    bool moving;

    float SpeedCache;
    float SpeedCapCache;
    float JumpCache;
    

    // Start is called before the first frame update
    void Start()
    { 
        SpeedCache = Speed;
        SpeedCapCache = SpeedCap;
        JumpCache = Jump;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), LayerMask.GetMask("Ground"));
        print(moving);
        Run();
        Jumping();
    }

    private void Jumping()
    {
        if (Input.GetAxis("Jump") != 0 && isGrounded)
        {
            velocity.x = Body.velocity.x;
            velocity.y = Jump * Input.GetAxis("Jump");
            Body.velocity = velocity;
        }
    }

    private void Run()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            StartCoroutine(AmIStopped());
            StartCoroutine(RunTimer());
            if (Speed < SpeedCap)
            {
                Speed += (Speed / SpeedCap) * 0.05f;
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

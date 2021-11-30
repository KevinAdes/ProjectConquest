using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HumanController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float damage;
    public float defense;
    public int expYield;

    [Header ("Components")]
    public Animator animator;
    public Collider2D collider2;
    public Rigidbody2D rigidbody2;

    public bool vulerable = true;

    public int right = -1;

    PlayerMovement Dracula;

    string STATE;
    
    // Start is called before the first frame update
    void Start()
    {
        Dracula = FindObjectOfType<PlayerMovement>();
        STATE = "Default";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    Human behavior

    default, set some waypoints. maybe left and right a bit to give it some wander, maybe specific locations in the level
    detection system, visual and audible, to see if dracula is near (future potential, sneak stat that bypasses this?)
    if dracula spotted, run opposite direction.

    bingo?
     */

    public float Get_Dmg()
    {
        return damage;
    }

    public float Get_Def()
    {
        return defense;
    }

    public float Get_Exp()
    {
        return expYield;
    }

    public void TakeDamage(float Dmg)
    {
        health -= Dmg;
        if(health <= 0)
        {
            Death();
        }
        StartCoroutine(invincibility());
    }

    private void Death()
    {
        animator.SetBool("Dead", true);
        vulerable = false;
        Dracula.AddXP(expYield);
        Physics2D.IgnoreCollision(collider2, Dracula.collider2);
        StartCoroutine(Decomposing());
    }

    private void Run()
    {
        
    }

    IEnumerator invincibility()
    {
        vulerable = false;
        yield return new WaitForSeconds(1);
        if(health > 0)
        {
            vulerable = true;
        }
    }

    IEnumerator Decomposing()
    {
        yield return new WaitForSeconds(60);
        animator.SetTrigger("Eaten");
    }

    public void Destroy()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

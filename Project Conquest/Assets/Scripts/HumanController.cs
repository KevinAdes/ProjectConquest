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

    public bool vulerable = true;


    PlayerMovement Dracula;

    
    // Start is called before the first frame update
    void Start()
    {
        Dracula = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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
}

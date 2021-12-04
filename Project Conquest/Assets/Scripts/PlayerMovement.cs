using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Stats")]
    public float maxHealth;
    public float damage;
    public float defense;
    public float speed;
    public float jump;
    public float attackModifier;

    [Header("Calculation Variables")]
    public float health;
    public float speedCap;
    public float acceleration;
    public float attackRange;

    [Header("Components")]
    public Rigidbody2D body;
    public Camera mainCamera;
    public Animator animator;
    public Transform attackPoint;
    public Collider2D collider2;

    [Header("Extras")]
    public LayerMask enemies;

    Vector2 velocity = new Vector2(0, 0);

    bool isGrounded;
    bool boost;
    bool moving;
    bool attack;
    bool drink;
    //functionally a bool that alternates between -1 and 1.
    int right = 1;

    float speedCache;
    float speedCapCache;
    float jumpCache;
    float scaleCache;
    float damageCache;

    public float blood = 0;

    string STATE;

    HumanController target;

    //^^^^VARIABLES^^^^
    //############################################################################
    //vvvvSCRIPTvvvv

    void Start()
    {
        STATE = "Default";
        health = maxHealth;
        speedCache = speed;
        speedCapCache = speedCap;
        jumpCache = jump;
        scaleCache = transform.localScale.x;
        damageCache = damage;
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.GetMask("Ground"));
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
        switch (STATE)
        {
            case "Default":
                Run();
                Jumping();
                Attack();
                Crouch();
                Direction();
                break;
            case "Drinking":
                Run();
                Jumping();
                Drink();
                Direction();
                Crouch();
                break;
        }
    }

    //Control Functions
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
            StartCoroutine(AmIStopped());
            StartCoroutine(RunTimer());
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
            moving = false;
            boost = false;
            jump = jumpCache;
            speedCap = speedCapCache;
            speed = (speed + speedCache) / 2;
            velocity.x = speed * Input.GetAxis("Horizontal");
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }
    }

    private void Attack()
    {
        //Slide
        if (Input.GetAxis("Fire1") != 0 && attack == false && animator.GetBool("Crouch") == true)
        {
            attack = true;
            animator.SetTrigger("Attack");
            velocity.x = speedCapCache * right;
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }

        //Attack
        if (Input.GetAxis("Fire1") != 0 && attack == false)
        {
            speed = speed / 2;
            attack = true;
            animator.SetTrigger("Attack");
        }

    }

    private void Drink()
    {
        if (Input.GetAxis("Fire1") != 0 && drink == false)
        {
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

    //
    //Animation Functions
    
    private void AttackCall()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);

        foreach (Collider2D enemy in hitEnemies)
        {
            HumanController human = enemy.GetComponent<HumanController>();
            if (human.vulerable == true)
            {
                human.TakeDamage(DamageCalculator(human.Get_Def()));

                Vector2 knockback = (enemy.transform.position - transform.position) + Vector3.up;
                print(knockback.x);
                human.animator.SetTrigger("Hit");
                enemy.gameObject.GetComponent<Rigidbody2D>().velocity += knockback * 5;
            }
        }
    }

    private void AttackDone()
    {
        attack = false;
        damage = damageCache;
    }

    private void DrinkDone()
    {
        drink = false;
    }

    private void SlideDone()
    {
        velocity.x = 0;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
    }

    private void StateSwitcher(string State)
    {
        STATE = State;
    }
    
    //Calculation Functions
    private float DamageCalculator(float Def)
    {
        if(attack == true)
        {
            damage += attackModifier;
        }
        float DMG = damage/Def + 1;
        return DMG;
    }

    public void AddXP(int gains)
    {
        blood += gains;
    }

    //Upgrade Functions
    public void UpgradeDamage()
    {
        if (blood > 0)
        {
            blood -= 1;
            damage += .1f;
            damage = (Mathf.RoundToInt(damage * 10));
            damage = damage / 10;
            damageCache += .1f;
            damageCache = (Mathf.RoundToInt(damageCache * 10));
            damageCache = damageCache / 10;
        }

    }

    public void UpgradeDefense()
    {
        if (blood > 0)
        {
            blood -= 1;
            defense += .1f;
            defense = (Mathf.RoundToInt(defense * 10));
            defense = defense / 10;
        }

    }

    public void UpgradeSpeed()
    {
        if (blood > 0)
        {
            blood -= 1;
            speed += .1f;
            speed = (Mathf.RoundToInt(speed * 10));
            speed = speed / 10;
            speedCache += .1f;
            speedCache = (Mathf.RoundToInt(speedCache * 10));
            speedCache = speedCache / 10;
            //speed cap may need to be upgraded by a higher amount
            speedCap += .1f;
            speedCap = (Mathf.RoundToInt(speedCap * 10));
            speedCap = speedCap / 10;
            speedCapCache += .1f;
            speedCapCache = (Mathf.RoundToInt(speedCapCache * 10));
            speedCapCache = speedCapCache / 10;
        }

    }

    public void UpgradeHealth()
    {
        if (blood > 0)
        {
            blood -= 1;
            maxHealth += 1;
        }

    }

    //I've grown tired of the runtimer boost eventually need to phase this out.
    //Enumerators
    IEnumerator RunTimer()
    {
        if (moving == true)
        {
            yield return new WaitForSeconds(10);

            if (boost == false && moving == true)
            {
                speedCap = speedCap * 1.25f;
                jump = jump * 1.5f;
                boost = true;
            }
            if (boost == true)
            {
                yield break;
            }
        }
        else
        {
            jump = jumpCache;
            speedCap = speedCapCache;
            yield break;
        }

    }
    IEnumerator AmIStopped()
    {
        float Gposition = GetComponent<Transform>().position.x;
        yield return new WaitForSeconds(2);
        if (Gposition == GetComponent<Transform>().position.x)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

    }

    //Engine Functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.gameObject.GetComponent<HumanController>().vulerable == true)
        {
            HumanController human = collision.gameObject.GetComponent<HumanController>();
            //Enemy Damage is greater than player defense
            if (human.Get_Dmg() > defense)
            {
                Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                body.velocity = knockback * 15;
                health -= 1;
                print("enemy damage is greater than player defense, player takes knockback and damage");
            }
            //Enemy Damage is less than player defense
            else
            {
                Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockback * 10;
                print("enemy damage is <||= player defense, enemy takes knockback");


            }
            //Player Damage is greater than Enemy Defense
            if (damage > collision.gameObject.GetComponent<HumanController>().Get_Def())
            {
                Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockback * 15;
                print("player damage is greater than enemy defense, enemy takes knockback and damage");
                collision.gameObject.GetComponent<HumanController>().TakeDamage(1);
            }
            //Player Damage is less than Enemy Defense
            else
            {
                Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                body.velocity = knockback * 10;
                print("player damage is <||= enemy defense, player takes knockback");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            target = collision.gameObject.GetComponent<HumanController>();
            StateSwitcher("Drinking");
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

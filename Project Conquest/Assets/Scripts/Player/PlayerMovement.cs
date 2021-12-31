using System;
using System.Collections;
using System.Collections.Generic;
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
    public Animator animator;
    public Transform attackPoint;
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
        pauseControl = FindObjectOfType<PauseControl>();
        velocity = new Vector2(0, 0);
        body.velocity = velocity;
        STATE = "Default";
        health = maxHealth;
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

        isGrounded = Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), LayerMask.GetMask("Ground"));
        
        switch (STATE)
        {
            case "Default":
                Run();
                Jumping();
                Attack();
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
            STATE = "Drink";
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

    private void AttackCall()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);

        foreach (Collider2D enemy in hitEnemies)
        {
            //when theres more enemy types, add a switch-case for different types or add a base class with a vulnerable stat
            //really need whatever I'm adding to field of view to automatically differentiate what type of thing were dealingwith

            if(enemy.GetComponent<HumanController>() != null)
            {
                HumanController human = enemy.GetComponent<HumanController>();
                if (human.vulerable == true)
                {
                    human.TakeDamage(DamageCalculator(damage, human.Get_Def(), attackModifier));

                    Vector2 knockback = (enemy.transform.position - transform.position) + Vector3.up;
                    human.animator.SetTrigger("Hit");
                     enemy.gameObject.GetComponent<Rigidbody2D>().velocity += knockback * 5;
                }
            }
            if (enemy.GetComponent<Entity>() != null)
            {
                print("hello");
                Entity entity = enemy.GetComponent<Entity>();
                entity.TakeDamage(DamageCalculator(damage, entity.Get_Def(), attackModifier));

                Vector2 knockback = (enemy.transform.position - transform.position) + Vector3.up;
                entity.animator.SetTrigger("Hit");
                enemy.gameObject.GetComponent<Rigidbody2D>().velocity += knockback * 5;
            }
        }
    }

    private void AttackDone()
    {
        attack = false;
    }

    private void DrinkDone()
    {
        STATE = "Default";
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
    //

    private float DamageCalculator(float dmg, float def, float modifier)
    {
        dmg += modifier;
        float DMG = dmg/def + 1;
        return DMG;
    }

    public void AddXP(int gains)
    {
        pauseControl.playerData.blood += gains;
    }

    private void Damage(Rigidbody2D target, Vector3 knockback, float dmg, float def)
    {
        target.velocity = knockback;
        health -= dmg;
    }
    //Engine Functions
    //

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HumanController>() != null)
        {
            if (collision.gameObject.layer == 10 && collision.gameObject.GetComponent<HumanController>().vulerable == true)
            {
                HumanController human = collision.gameObject.GetComponent<HumanController>();
                //Enemy Damage is greater than player defense
                if (human.Get_Dmg() > defense)
                {
                    Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                    //Damage(body, knockback, human.Get_Dmg());
                    body.velocity = knockback * 15;
                    health -= 1;
                    //print("enemy damage is greater than player defense, player takes knockback and damage");
                }
                //Enemy Damage is less than player defense
                else
                {
                    Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockback * 10;
                    //print("enemy damage is <||= player defense, enemy takes knockback");


                }
                //Player Damage is greater than Enemy Defense
                if (damage > collision.gameObject.GetComponent<HumanController>().Get_Def())
                {
                    Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = knockback * 15;
                    //print("player damage is greater than enemy defense, enemy takes knockback and damage");
                    collision.gameObject.GetComponent<HumanController>().TakeDamage(1);
                }
                //Player Damage is less than Enemy Defense
                else
                {
                    Vector3 knockback = collision.transform.position - transform.position + Vector3.up * 0.33f;
                    body.velocity = knockback * 10;
                    //print("player damage is <||= enemy defense, player takes knockback");
                }
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            manager.LoadLevel("Map");
        }

        if (collision.gameObject.layer == 10)
        {
            target = collision.gameObject.GetComponent<HumanController>();
            StateSwitcher("Drink");
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

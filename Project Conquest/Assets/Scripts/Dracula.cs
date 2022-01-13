using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyManager;

public class Dracula : MonoBehaviour
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

    public float speedCache;
    public float speedCapCache;

    bool attack;
    bool drink;
    public Animator animator;
    PlayerMovement me;
    PauseControl pauseControl;


    string STATE;

    public Transform attackPoint;
    public float attackRange;

    Vector2 velocity;

    public LayerMask enemies;

    GameManager manager;

    Entity target;

    public func checking;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        pauseControl = FindObjectOfType<PauseControl>();
        health = manager.playerData.currentHealth;
        me = GetComponent<PlayerMovement>();
        me.speed = speed;
        me.jump = jump;
        me.speedCap = speedCap;
        me.acceleration = acceleration;
        me.speedCache = speedCache;
        me.speedCapCache = speedCapCache;
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Scavenge();
        if (checking != null)
        {
            print("...");
            checking.Invoke();
        }
    }


    private void Attack()
    {

        //Attack
        if (Input.GetAxis("Fire1") != 0 && attack == false)
        {
            me.speed = me.speed / 2;
            attack = true;
            animator.SetTrigger("Attack");
        }

    }

    public void AttackCall()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemies);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.parent.GetComponent<DamageSystem>() != null)
            {

                Entity target = enemy.transform.parent.GetComponent<Entity>();
                DamageSystem damageSystem = enemy.transform.parent.GetComponent<DamageSystem>();
                if (damageSystem.vulnerable == true)
                {
                    Vector2 knockback = ((enemy.transform.position - transform.position) + Vector3.up);
                    damageSystem.TakeDamage(damageSystem.body, knockback, damageSystem.DamageCalculator(damage, target.defense, attackModifier));

                    target.animator.SetTrigger("Hit");
                    enemy.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity += knockback * 5;
                }
            }
        }
    }

    public void AttackDone()
    {
        attack = false;
    }


    private void Scavenge()
    {
        if (target != null && target.dead == true)
        {

            if (Input.GetAxis("Fire1") != 0 && drink == false)
            {
                me.StateSwitcher("Scavenging");
                drink = true;
                //Gain extra exp from victim
                animator.SetTrigger("Drink");
                AddXP(target.expYield);
                target.animator.SetTrigger("Destroy");
            }
        }

    }

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
        if (collision.gameObject.layer == 17)
        {
            manager.playerData.currentHealth = health;
            manager.LoadLevel("Map");
        }

        if (collision.gameObject.layer == 10)
        {
            if (collision.transform.parent.gameObject.GetComponent<Entity>() != null)
            {
                target = collision.transform.parent.gameObject.GetComponent<Entity>();
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

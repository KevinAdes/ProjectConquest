using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dracula : MonoBehaviour
{

    public float damage;
    public float defense;

    bool attack;
    bool drink;
    public Animator animator;
    PlayerMovement me;


    public Transform attackPoint;
    public float attackRange;
    public float attackModifier;

    Vector2 velocity;

    public LayerMask enemies;

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<PlayerMovement>();
        damage = me.damage;
        defense = me.defense;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
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
                if (damageSystem.vulerable == true)
                {
                    Vector2 knockback = ((enemy.transform.position - transform.position) + Vector3.up) * 5;
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


    private void Drink()
    {
        if (Input.GetAxis("Fire1") != 0 && drink == false)
        {
            me.StateSwitcher("Drinking");
            drink = true;
            //Gain extra exp from victim
            animator.SetTrigger("Drink");
            //me.AddXP(target.expYield);
            //target.animator.SetTrigger("Eaten");
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

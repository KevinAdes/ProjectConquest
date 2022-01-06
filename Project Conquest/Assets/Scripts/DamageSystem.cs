using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    Entity entity;
    HumanController human;
    PlayerMovement Dracula;

    public Rigidbody2D body;

    public float health;
    public float damage;
    public float defense;
    public int expYield;

    public bool vulerable = true;


    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        if (GetComponent<HumanController>() != null)
        {
            human = GetComponent<HumanController>();
            health = human.health;
            damage = human.damage;
            defense = human.defense;
        }
        if (GetComponent<Entity>() != null)
        {
            entity = GetComponent<Entity>();
            health = entity.health;
            damage = entity.damage;
            defense = entity.defense;
        }
        if (GetComponent<PlayerMovement>() != null)
        {
            Dracula = GetComponent<PlayerMovement>();
            health = Dracula.health;
            damage = Dracula.damage;
            defense = Dracula.defense;
        }
    }


    public float DamageCalculator(float dmg, float def, float modifier)
    {
        dmg += modifier;
        float DMG = dmg / def + 1;
        return DMG;
    }
    public float DamageCalculator(float dmg, float def)
    {
        float DMG = dmg / def + 1;
        return DMG;
    }

    //Add knockback modifier
    public void TakeDamage(Rigidbody2D target, Vector2 knockback, float dmg)
    {
        target.velocity += knockback;
        health -= dmg;
        if (entity != null)
        {
            entity.detected = true;
        }
        if (health <= 0)
        {
            if (GetComponent<HumanController>() != null)
            {
                GetComponent<HumanController>().Death();
            }
            else
            {
                StartCoroutine(Destroy());
            }
        }
        StartCoroutine(invincibility());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamageSystem>() != null)
        {
            DamageSystem target = collision.gameObject.GetComponent<DamageSystem>();
            if (target.vulerable == true)
            {
                if (target.damage > defense)
                {
                    Vector3 knockback = transform.position - target.transform.position + Vector3.up * 0.33f;
                    TakeDamage(body, knockback, DamageCalculator(target.damage, defense));
                }
            }
        }

    }

    IEnumerator invincibility()
    {
        vulerable = false;
        yield return new WaitForSeconds(1);
        if (health > 0)
        {
            vulerable = true;
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }


}

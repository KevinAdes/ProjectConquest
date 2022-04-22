using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    Entity entity;
    Interactable interactable;
    Dracula dracula;

    [SerializeField]
    Rigidbody2D body;
    
    float health;
    float damage;
    float defense;
    int expYield;

    bool vulnerable = true;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        if (GetComponent<Entity>() != null)
        {
            entity = GetComponent<Entity>();
            health = entity.GetHealth();
            damage = entity.GetDamage();
            defense = entity.GetDefense();
        }
        if (GetComponent<Dracula>() != null)
        {
            dracula = GetComponent<Dracula>();
            health = dracula.GetHealth();
            damage = dracula.GetDamage();
            defense = dracula.GetDefense();
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
        if (target.bodyType != RigidbodyType2D.Static)
        {
            target.velocity += knockback;
        }
        health -= dmg;

        if(dracula != null)
        {
            dracula.SetHealth(dracula.GetHealth() - dmg);
        }
        if (entity != null)
        {
            entity.SetDetected(true);
            entity.SetHealth(entity.GetHealth() - dmg);
        }
        if (health <= 0)
        {
            if (entity != null)
            {
                entity.SetDead(true);
                GetComponent<Entity>().Death();
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
        if (collision.gameObject.GetComponent<DamageSystem>() != null && collision.transform.tag == "attack")
        {
            DamageSystem target = collision.gameObject.GetComponent<DamageSystem>();
            if (target.vulnerable == true)
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
        vulnerable = false;
        yield return new WaitForSeconds(1);
        if (health > 0)
        {
            vulnerable = true;
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }

    //Getters and Setters
    public bool GetVulnerable()
    {
        return vulnerable;
    }

    public void SetVulnerable(bool b)
    {
        vulnerable = b;
    }

    public Rigidbody2D GetBody()
    {
        return body;
    }
    
    public Entity GetEntity()
    {
        return entity;
    }

    public float GetDefense()
    {
        return defense;
    }
}

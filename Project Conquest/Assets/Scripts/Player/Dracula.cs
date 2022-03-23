using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SkillsList;

public enum states
{
    DEFAULT,
    DIALOGUE,
    SCAVENGING
}

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
    InventoryObject inventory;

    public states STATE;

    public Transform attackPoint;
    public float attackRange;

    Vector2 velocity;

    public LayerMask enemies;

    GameManager manager;

    Entity target;

    UnityEvent ProcessDefault = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        pauseControl = FindObjectOfType<PauseControl>();
        pauseControl.SetDracula(this);
        pauseControl.Calibrate();
        inventory = pauseControl.inventory;
        health = manager.playerData.currentHealth;
        me = GetComponent<PlayerMovement>();
        me.speed = speed;
        me.jump = jump;
        me.speedCap = speedCap;
        me.acceleration = acceleration;
        me.speedCache = speedCache;
        me.speedCapCache = speedCapCache;

        STATE = states.DEFAULT;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (STATE)
        {
            case states.DEFAULT:

                ExecuteProcessOne();
                Scavenge();
                break;
            case states.DIALOGUE:
                break;
        }
    }

    private void ExecuteProcessOne()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            pauseControl.GetProcess1().Invoke();
        }
    }
    
    public void Attack()
    {
        if(attack == false)
        {
            attack = true;
            me.speed = me.speed / 2;
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
            if (enemy.GetComponent<Interactable>() != null)
            {
                enemy.GetComponent<Interactable>().Death();
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
                me.StateSwitcher(states.SCAVENGING);
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
        STATE = states.DEFAULT;
        drink = false;
    }

    public void StateSwitcher(states State)
    {
        STATE = State;
    }


    public void AddXP(int gains)
    {
        pauseControl.GetPlayerData().AddBlood(gains);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17)
        {
            //this shit is why i hate getters and setters. why cant i just make this shit public Oh nO yOu CAnt HaVE GlobBal VaRiaAbLEs why. why not. i did it until now and its been fine. f this shit. 
            FindObjectOfType<Level>().GetData().SetRight(collision.GetComponent<LevelLoader>().GetRight());
            if (collision.GetComponent<LevelLoader>().GetCoords() != Vector3.zero)
            {
                manager.playerData.currentHealth = health;
                manager.LoadLevel("Map", collision.GetComponent<LevelLoader>().GetCoords());
            }
            else
            {
                manager.playerData.currentHealth = health;
                manager.LoadLevel(collision.GetComponent<LevelLoader>().GetID());
            }
        }

        if (collision.gameObject.layer == 10)
        {
            if (collision.transform.parent.gameObject.GetComponent<Entity>() != null)
            {
                target = collision.transform.parent.gameObject.GetComponent<Entity>();
            }
        }

        if (collision.gameObject.layer == 18)
        {
            inventory.AddItem(collision.gameObject.GetComponent<GameItem>().item, 1);
            pauseControl.UpdateInventory();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            target = null;
            StateSwitcher(states.DEFAULT);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Setters and Getters

    public InventoryObject GetInventory()
    {
        return inventory;
    }

    public bool GetAttack()
    {
        return attack;
    }

    public void SetAttack(bool b)
    {
        attack = b;
    }

    public void OnDestroy()
    {
        if(health <= 0)
        {

            manager.GetComponent<Animator>().SetTrigger("GameOver");
            pauseControl.GetPlayerData().SetBlood(0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public enum states
{
    DEFAULT,
    DIALOGUE,
    SCAVENGING
}

public class Dracula : MonoBehaviour
{
    float maxHealth;
    float damage;
    float defense;
    float speed;
    float jump;
    float attackModifier;

    float health;
    float acceleration;

    bool attack;
    bool drink;
    Animator animator;
    PlayerMovement me;
    PauseControl pauseControl;
    InventoryObject inventory;

    states STATE;

    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    float attackRange;

    Vector2 velocity;

    [SerializeField]
    LayerMask enemies;

    [SerializeField]
    SpriteRenderer alert;

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
        inventory = pauseControl.GetInventory();
        health = manager.GetPlayerData().GetCurrentHealth();
        me = GetComponent<PlayerMovement>();
        me.SetSpeed(speed);
        me.SetJump(jump);
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
                ExecuteProcessTwo();
                ExecuteProcessThree();
                ExecuteProcessFour();
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
            pauseControl.GetProcess1()?.Invoke();
        }
    }

    private void ExecuteProcessTwo()
    {
        if (Input.GetAxis("Fire2") != 0)
        {
            pauseControl.GetProcess2()?.Invoke();
        }
    }

    private void ExecuteProcessThree()
    {
        if (Input.GetAxis("Fire3") != 0)
        {
            pauseControl.GetProcess3()?.Invoke();
        }
    }

    private void ExecuteProcessFour()
    {
        if (Input.GetAxis("Fire4") != 0)
        {
            pauseControl.GetProcess4()?.Invoke();
        }
    }

    public void Attack()
    {
        if(attack == false)
        {
            attack = true;
            me.SetSpeed(me.GetSpeed() / 2);
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
                if (damageSystem.GetVulnerable())
                {
                    Vector2 knockback = ((enemy.transform.position - transform.position) + Vector3.up);
                    damageSystem.TakeDamage(damageSystem.GetBody(), knockback, damageSystem.DamageCalculator(damage, target.GetDefense(), attackModifier));

                    target.GetAnimator().SetTrigger("Hit");
                    enemy.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity += knockback * 5;
                }
            }
            if (enemy.GetComponent<Interactable>() != null)
            {
                enemy.GetComponent<Interactable>().Death();
            }
            if(enemy.GetComponent<Destructable>() != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    public void AttackDone()
    {
        attack = false;
    }


    private void Scavenge()
    {
        if (target != null && target.GetDead() == true)
        {

            if (Input.GetAxis("Fire1") != 0 && drink == false)
            {
                me.SetState(states.SCAVENGING);
                drink = true;
                //Gain extra exp from victim
                animator.SetTrigger("Drink");
                AddXP((int)target.GetExpYield());
                target.GetAnimator().SetTrigger("Destroy");
            }
        }

    }

    private void DrinkDone()
    {
        STATE = states.DEFAULT;
        drink = false;
    }

    public void AddXP(int gains)
    {
        pauseControl.GetPlayerData().AddBlood(gains);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17)
        {
            FindObjectOfType<Level>().GetData().SetRight(collision.GetComponent<LevelLoader>().GetRight());
            if (collision.GetComponent<LevelLoader>().GetCoords() != Vector3.zero)
            {
                manager.GetPlayerData().SetCurrentHealth(health);
                manager.LoadLevel("Map", collision.GetComponent<LevelLoader>().GetCoords());
            }
            else
            {
                manager.GetPlayerData().SetCurrentHealth(health);
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
            inventory.AddItem(collision.gameObject.GetComponent<GameItem>().GetItem(), 1);
            pauseControl.UpdateInventory();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            target = null;
            SetState(states.DEFAULT);
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
            manager.SetMapula(Vector3.zero);
            manager.GetComponent<Animator>().SetTrigger("GameOver");
            FindObjectOfType<GameManager>().GetFlags().GetType().GetField("Respawn").SetValue(FindObjectOfType<GameManager>().GetFlags(), false);
            pauseControl.GetPlayerData().SetBlood(0);
        }
    }

    public void ActivateAlert()
    {
        alert.enabled = true;
    }

    public void DeactivateAlert()
    {
        alert.enabled = false;
    }

    //Getters and Setters
    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float f)
    {
        health = f;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float f)
    {
        maxHealth = f;
    }

    public float GetDamage()
    {
        return damage;
    }
    public void SetDamage(float f)
    {
        damage = f;
    }

    public float GetDefense()
    {
        return defense;
    }
    public void SetDefense(float f)
    {
        defense = f;
    }

    public float GetAttackModifier()
    {
        return attackModifier;
    }

    public void SetAttackModifier(float f)
    {
        attackModifier = f;
    }
    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float f)
    {
        speed = f;
    }

    public float GetJump()
    {
        return jump;
    }

    public void SetJump(float f)
    {
        jump = f;
    }
    public Animator GetAnimator()
    {
        return animator;
    }

    public states GetState()
    {
        return STATE;
    }

    public void SetState(states s)
    {
        STATE = s;
    }


}

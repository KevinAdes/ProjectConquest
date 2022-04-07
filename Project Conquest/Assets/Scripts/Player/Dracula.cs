using System.Collections;
using System.Collections.Generic;
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
    float speedCap;
    float acceleration;

    float speedCache;
    float speedCapCache;

    bool attack;
    bool drink;
    Animator animator;
    PlayerMovement me;
    PauseControl pauseControl;
    InventoryObject inventory;

    states STATE;

    Transform attackPoint;
    float attackRange;

    Vector2 velocity;

    [SerializeField]
    LayerMask enemies;

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
                me.StateSwitcher(states.SCAVENGING);
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

            manager.GetComponent<Animator>().SetTrigger("GameOver");
            pauseControl.GetPlayerData().SetBlood(0);
        }
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
    public float GetSpeedCap()
    {
        return speedCap;
    }

    public void SetSpeedCap(float f)
    {
        speedCap = f;
    }
    public float GetSpeedCache()
    {
        return speed;
    }

    public void SetSpeedCache(float f)
    {
        speedCache = f;
    }
    public float GetSpeedCapCache()
    {
        return speed;
    }

    public void SetSpeedCapCache(float f)
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

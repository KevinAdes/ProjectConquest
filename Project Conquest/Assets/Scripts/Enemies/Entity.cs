using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SkillsList;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [SerializeField]
    string myName;

    [Header("Stats")]
    [SerializeField]
    float health, damage, defense, speed, expYield;

    [Header("Components")]
    [SerializeField]
    Animator animator;

    [SerializeField]
    bool important;
    int ID;
    
    bool detected = false;
    bool stunned;
    bool dead;

    int direction = 1;

    GameManager manager;

    [SerializeField]
    EnemySkill[] skillSet;

    public void OnEnable()
    {
        if (dead == true)
        {
            animator.Play("Off");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public float Get_Exp()
    {
        return expYield;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 17)
        {
            StartCoroutine(Decomposing());
        }
    }

    public void Death()
    {
        animator.SetTrigger("PowerDown");
        if(important == true)
        {
            FindObjectOfType<GameManager>().markDead(ID, SceneManager.GetActiveScene().name);
        }
        if(skillSet.Length > 0)
        {
            FindObjectOfType<GameManager>().AddSkill(ID, SceneManager.GetActiveScene().name);
        }
        GetComponent<DamageSystem>().SetVulnerable(false);
        StartCoroutine(Decomposing());
    }

    IEnumerator Decomposing()
    {
        yield return new WaitForSeconds(60);
        Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public string GetName()
    {
        return myName;
    }

    public EnemySkill[] GetSkills()
    {
        return skillSet;
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

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float f)
    {
        speed = f;
    }
    public float GetExpYield()
    {
        return expYield;
    }

    public void SetExpYield(float f)
    {
        expYield = f;
    }

    public int GetID()
    {
        return ID;
    }

    public void SetID(int i)
    {
        ID = i;
    }

    public int GetDirection()
    {
        return direction;
    }
    public void SetDirection(int i)
    {
        direction = i;
    }

    public bool GetImportant()
    {
        return important;
    }

    public bool GetDetected()
    {
        return detected;
    }
    public void SetDetected(bool b)
    {
        detected = b;
    }

    public bool GetDead()
    {
        return dead;
    }

    public void SetDead(bool b)
    {
        dead = b;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}

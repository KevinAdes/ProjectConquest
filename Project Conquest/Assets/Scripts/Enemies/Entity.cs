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
    public float health;
    public float damage;
    public float defense;
    public float speed;
    public int expYield;

    [Header("Components")]
    public Animator animator;

    public bool important;
    public int ID;

    [HideInInspector]
    public bool detected = false;
    public bool stunned;
    public bool dead;

    [HideInInspector]
    public int direction = 1;

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
        GetComponent<DamageSystem>().vulnerable = false;
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

}

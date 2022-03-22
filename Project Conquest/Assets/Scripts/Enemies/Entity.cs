using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnemySkills;

public class Entity : MonoBehaviour
{
    public string myName;
    public List<func> skills = new List<func>();
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

    public void OnEnable()
    {
        if (dead == true)
        {
            animator.Play("Off");
        }
    }

    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
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
            manager.markDead(ID, SceneManager.GetActiveScene().name);
        }
        if(skills.Count > 0)
        {
            manager.AddSkill(ID, SceneManager.GetActiveScene().name);
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

    public void AddSkill(func skillToAdd)
    {
        skills.Add(skillToAdd);
    }
}

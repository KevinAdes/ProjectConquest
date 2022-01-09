using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
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

    public void SmallHop()
    {
        transform.GetComponent<Rigidbody2D>().velocity += new Vector2(.33f, 0f);
    }

    public void Death()
    {
        animator.SetTrigger("PowerDown");
        if(important == true)
        {
            manager.markDead(ID, SceneManager.GetActiveScene().name);
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnemyManager;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoboGuy : MonoBehaviour
{
    //TODO Check for unneeded code 
    [Header("Stats")]
    public int expYield;

    [Header("Components")]
    public Animator animator;
    public Collider2D collider2;
    public CircleCollider2D trigger;
    public Rigidbody2D body;

    [Header("Calculation Variables")]
    public float wait;
    [Range(0, 10)]
    public float courage;
    float suspicion = 0;
    public float alertRange;


    [HideInInspector]
    bool courageRolled = false;
    bool alerted = false;
    public bool dead = false;

    [HideInInspector]
    public int direction = 1;
    int flip;
    public int ID;

    PlayerMovement player;

    string STATE;

    Vector3[] waypoints;
    Vector2 velocity = new Vector2(0, 0);

    float scaleCache;

    GameManager manager;

    Entity entity;
    
    func dele;

    public void Awake()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        entity = GetComponent<Entity>();
        entity.myName = "RoboGuy";
        dele = manager.skillsList.SmallHop;
        entity.AddSkill(dele);
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        if (courage == 0)
        {
            courage = Random.Range(0, 10);
        }
        animator = GetComponent<Animator>();
        collider2 = GetComponentInChildren<Collider2D>();
        trigger = GetComponentInChildren<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        scaleCache = transform.localScale.x;
        player = FindObjectOfType<PlayerMovement>();
        STATE = "Default";
        expYield = expYield + Mathf.RoundToInt(courage / 5);

        StartCoroutine(ChangeMove());
    }

    // Update is called once per frame
    void Update()
    {
        switch (STATE)
        {
            case "Default":
                CheckThings();
                break;
            case "Scared":
                Run();
                CheckThings();
                break;
            case "Dead":
                StopCoroutine(ChangeMove());
                GetComponent<Wanderer>().enabled = false;
                GetComponent<ContinuousMovement>().enabled = false;
                break;
        }
    }
    private void CheckThings()
    {
        if (entity.detected)
        {
            suspicion = 5;
            Alert();
        }
        if (entity.dead)
        {
            StateSwitcher("Dead");
        }
    }
    public void Alert()
    {
        if(alerted == false)
        {
            SmallHop();
            StateSwitcher("Scared");
            StopAllCoroutines();
            alerted = true;
            Collider2D[] friends = Physics2D.OverlapCircleAll(transform.position, alertRange);

            foreach (Collider2D guy in friends)
            {
                if (guy.GetComponent<DefenseSystem>() != null)
                {
                    guy.GetComponent<DefenseSystem>().PowerOn();
                }
                if (guy.transform.parent != null && guy.transform.parent.GetComponent<RoboGuy>() != null)
                {
                    guy.transform.parent.GetComponent<RoboGuy>().Alert();
                }
            }
        }
    }


    public float Get_Exp()
    {
        return expYield;
    }

    private void Run()
    {
        if (courageRolled == false)
        {
            int random = Random.Range(0, 10);
            if (random < courage)
            {
                flip = -1;
            }
            else
            {
                flip = 1;
            }
            courageRolled = true;

        }
        bool facingdirection = (transform.position.x - player.transform.position.x >= 0);
        if (!facingdirection)
        {
            transform.localScale = new Vector3(scaleCache * -1 * flip, transform.localScale.y, transform.localScale.z);
            direction = -1;
        }
        if (facingdirection)
        {
            transform.localScale = new Vector3(scaleCache * flip, transform.localScale.y, transform.localScale.z);
            direction = 1;
        }
        velocity.x = entity.speed * direction * flip * Time.deltaTime * 10;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 && suspicion < 1)
        {
            suspicion += .1f * Time.deltaTime;
            if (suspicion >= 5f)
            {
                Alert();
            }
        }
    }

    IEnumerator ChangeMove()
    {
        if (GetComponent<ContinuousMovement>().enabled == true)
        {
            float var = Random.Range(50, 70);
            yield return new WaitForSeconds(var);
            GetComponent<Wanderer>().enabled = true;
            GetComponent<Wanderer>().active = true;
            GetComponent<Wanderer>().Gas();
            GetComponent<ContinuousMovement>().enabled = false;
            StartCoroutine(ChangeMove());
            yield break;
        }
        if (GetComponent<Wanderer>().enabled == true)
        {
            float iable = Random.Range(4, 8);
            yield return new WaitForSeconds(iable);
            GetComponent<Wanderer>().active = false;
            GetComponent<Wanderer>().enabled = false;
            GetComponent<ContinuousMovement>().enabled = true;
            StartCoroutine(ChangeMove());
            yield break;
        }
    }

    public void Freeze()
    {
        StopCoroutine(ChangeMove());
        GetComponent<Wanderer>().active = false;
        GetComponent<Wanderer>().enabled = false;
        GetComponent<ContinuousMovement>().enabled = false;
    }

    public void UnFreeze()
    {
        GetComponent<Wanderer>().active = true;
        GetComponent<Wanderer>().enabled = true;
        GetComponent<Wanderer>().Gas();
        StopCoroutine(ChangeMove());
    }

    public void SmallHop()
    {
        transform.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 3f);
    }


    public void StateSwitcher(string State)
    {
        STATE = State;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SkillsList;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoboGuy : MonoBehaviour
{
    //TODO Check for unneeded code 
    [SerializeField]
    int expYield;
    
    Animator animator;
    Collider2D collider2;
    CircleCollider2D trigger;
    Rigidbody2D body;

    [Header("Calculation Variables")]
    [SerializeField]
    float wait;
    [SerializeField]
    [Range(0, 10)]
    float courage;
    float suspicion = 0;
    [SerializeField]
    float alertRange;

    [SerializeField]
    bool wanderer;
    bool courageRolled = false;
    
    bool alerted = false;

    int direction = 1;
    int flip;
    int ID;

    PlayerMovement player;

    string STATE;

    Vector3[] waypoints;
    Vector2 velocity = new Vector2(0, 0);

    float scaleCache;

    GameManager manager;

    Entity entity;

    public void Awake()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        entity = GetComponent<Entity>();
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
        if (!wanderer)
        {
            StartCoroutine(ChangeMove());
        }
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
        if (entity.GetDetected())
        {
            suspicion = 5;
            Alert();
        }
        if (entity.GetDead())
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
                print("there are friends");
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
        velocity.x = entity.GetSpeed() * direction * flip * Time.deltaTime * 10;
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
            GetComponent<Wanderer>().SetActive(true);
            GetComponent<Wanderer>().Gas();
            GetComponent<ContinuousMovement>().enabled = false;
            StartCoroutine(ChangeMove());
            yield break;
        }
        if (GetComponent<Wanderer>().enabled == true)
        {
            float iable = Random.Range(4, 8);
            yield return new WaitForSeconds(iable);
            GetComponent<Wanderer>().SetActive(false);
            GetComponent<Wanderer>().enabled = false;
            GetComponent<ContinuousMovement>().enabled = true;
            StartCoroutine(ChangeMove());
            yield break;
        }
    }

    public void Freeze()
    {
        StopCoroutine(ChangeMove());
        if(GetComponent<Wanderer>() != null)
        {
            GetComponent<Wanderer>().SetActive(false);
            GetComponent<Wanderer>().enabled = false;
        }
        if(GetComponent<ContinuousMovement>() != null)
        {
            GetComponent<ContinuousMovement>().enabled = false;
        }
    }

    public void UnFreeze()
    {
        if (GetComponent<Wanderer>() != null)
        {
            GetComponent<Wanderer>().SetActive(true);
            GetComponent<Wanderer>().enabled = true;
            GetComponent<Wanderer>().Gas();
            StopCoroutine(ChangeMove());
        }
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

    public bool GetAlerted()
    {
        return alerted;
    }

    public void SetAlerted(bool b)
    {
        alerted = b;
    }
}

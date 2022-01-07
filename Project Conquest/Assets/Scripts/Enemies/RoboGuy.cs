using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoboGuy : MonoBehaviour
{
    [Header("Stats")]
    public int expYield;

    [Header("Components")]
    public Animator animator;
    public Collider2D collider2;
    public CircleCollider2D trigger;
    public Rigidbody2D body;

    [Header("Calculation Variables")]
    public float speed;
    public float wait;
    [Range(0, 10)]
    public float courage;
    float suspicion = 0;

    [HideInInspector]
    bool courageRolled = false;
    public bool stunned = false;
    bool alerted;
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

    bool enumerator = false;

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

        StartCoroutine(ChangeMove());
    }

    // Update is called once per frame
    void Update()
    {
        switch (STATE)
        {
            case "Default":
                break;
            case "Scared":
                Run();
                break;
            case "Dead":
                break;
        }
    }

    IEnumerator ChangeMove()
    {
        yield return new WaitForSeconds(60f);
        if (GetComponent<ContinuousMovement>().enabled == true)
        {
            GetComponent<Wanderer>().enabled = true;
            GetComponent<ContinuousMovement>().enabled = false;
            StartCoroutine(ChangeMove());
            yield break;
        }
        if (GetComponent<Wanderer>().enabled == true)
        {
            GetComponent<Wanderer>().enabled = false;
            GetComponent<ContinuousMovement>().enabled = true;
            StartCoroutine(ChangeMove());
            yield break;
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
        if (!stunned)
        {
            velocity.x = speed * direction * flip;
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 13)
        {
            suspicion += .1f * Time.deltaTime;
            print(suspicion);
        }
    }
}

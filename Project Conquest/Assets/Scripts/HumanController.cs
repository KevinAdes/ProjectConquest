using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class HumanController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float damage;
    public float defense;
    public int expYield;

    [Header ("Components")]
    public Animator animator;
    public Collider2D collider2;
    public Rigidbody2D rigidbody2;
    public Transform Path;

    [Header("Calculation Variables")]
    public float speed;
    public float wait;
    public float courage;

    [HideInInspector]
    public bool vulerable = true;
    public bool spooked = false;

    [HideInInspector]
    public int right = 1;
    int currentPoint = 0;

    PlayerMovement Dracula;

    string STATE;

    Vector3[] waypoints;
    Vector2 velocity = new Vector2 (0,0);

    float scaleCache;

    // Start is called before the first frame update
    void Start()
    {
        scaleCache = transform.localScale.x;
        Dracula = FindObjectOfType<PlayerMovement>();
        STATE = "Default";
        waypoints = new Vector3[Path.childCount];

        for (int i = 0; i < waypoints.Length; i++){
            waypoints[i] = Path.GetChild(i).position;
        }
        StartCoroutine(Wander(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        print(STATE);
        switch(STATE)
        {
            case "Default":
                Direction();
                break;
            case "Scared":
                Run();
                break;
            case "Dead":
                break;
        }
    }

    //Getter Functions
    public float Get_Dmg()
    {
        return damage;
    }

    public float Get_Def()
    {
        return defense;
    }

    public float Get_Exp()
    {
        return expYield;
    }

    //animation Functions

    private void SmallHop()
    {
        
        rigidbody2.velocity = new Vector2(0, 4);
        STATE = "Scared";
    }

    //Control Functions
    private void Direction()
    {
        bool facingRight = (transform.position.x - waypoints[currentPoint].x <= 0);
        if (!facingRight)
            {
                transform.localScale = new Vector3(scaleCache * -1, transform.localScale.y, transform.localScale.z);
            print(transform.localScale);
                right = -1;
            }
        if (facingRight)
            {
                transform.localScale = new Vector3(scaleCache, transform.localScale.y, transform.localScale.z);
            print(transform.localScale);
                right = 1;
            }
    }

    private void Run()
    {
        bool facingRight = (transform.position.x - Dracula.transform.position.x >= 0);
        if (!facingRight)
        {
            transform.localScale = new Vector3(scaleCache * -1, transform.localScale.y, transform.localScale.z);
            right = -1;
        }
        if (facingRight)
        {
            transform.localScale = new Vector3(scaleCache, transform.localScale.y, transform.localScale.z);
            right = 1;
        }
        velocity.x = speed * right;
        velocity.y = rigidbody2.velocity.y;
        rigidbody2.velocity = velocity;
    }

    IEnumerator invincibility()
    {
        vulerable = false;
        yield return new WaitForSeconds(1);
        if(health > 0)
        {
            vulerable = true;
        }
    }

    IEnumerator Decomposing()
    {
        yield return new WaitForSeconds(60);
        animator.SetTrigger("Eaten");
    }

    IEnumerator Wander(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        currentPoint = 1;
        Vector3 targetWaypoint = waypoints[currentPoint];
        while (true)
        {
            Vector3 positionStore = transform.position;
            var dist = Vector3.Distance(positionStore, targetWaypoint);
            transform.position = Vector2.Lerp(transform.position, targetWaypoint, speed*(Time.deltaTime/dist) );
            if (spooked) { break; }
            if (Mathf.Abs(transform.position.x - targetWaypoint.x) < .1)
            {
                currentPoint = (currentPoint + 1) % waypoints.Length;
                targetWaypoint = waypoints[currentPoint];
                yield return new WaitForSeconds(wait);
            }
            yield return null;
        }
    }

    //Death Functions
    public void TakeDamage(float Dmg)
    {
        health -= Dmg;
        if (health <= 0)
        {
            Death();
        }
        StartCoroutine(invincibility());
    }

    private void Death()
    {
        animator.SetBool("Dead", true);
        STATE = "dead";
        vulerable = false;
        Physics2D.IgnoreCollision(collider2, Dracula.collider2);
        StartCoroutine(Decomposing());
    }

    public void Destroy()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

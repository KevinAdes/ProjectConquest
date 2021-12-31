using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float damage;
    public float defense;
    public int expYield;

    [Header("Components")]
    public Animator animator;
    public Collider2D collider2;
    public Rigidbody2D rigidbody2;


    [HideInInspector]
    public bool vulerable = true;
    public bool detected = false;
    public bool stunned = false;

    [HideInInspector]
    public int direction = 1;
    int flip;

    PlayerMovement Dracula;

    string STATE;

    float scaleCache;

    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        STATE = "DEFAULT";
        scaleCache = transform.localScale.x;
        Dracula = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
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


    IEnumerator invincibility()
    {
        vulerable = false;
        yield return new WaitForSeconds(1);
        if (health > 0)
        {
            vulerable = true;
        }
    }

    //Death Functions
    //

    public void TakeDamage(float Dmg)
    {
        print("check");
        detected = true;
        STATE = "Scared";
        health -= Dmg;
        if (health <= 0)
        {
            if (GetComponent<HumanController>() != null)
            {
                GetComponent<HumanController>().Death();
            }
            else
            {
                Destroy();
            }
        }
        StartCoroutine(invincibility());
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

}

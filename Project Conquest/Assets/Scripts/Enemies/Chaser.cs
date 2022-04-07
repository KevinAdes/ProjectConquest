using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField]
    int speed;
    [SerializeField]
    Rigidbody2D body;
    Vector2 velocity;
    int direction;

    Entity me;
    PlayerMovement player;
    float scaleCache;

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Entity>();
        player = FindObjectOfType<PlayerMovement>();
        scaleCache = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (me.GetDead() == false)
        {
            switch (me.GetDetected())
            {
                case true:
                    Run();
                    break;
                case false:
                    break;
            }
        }
    }

    private void Run()
    {
        bool right;
        if (transform.position.x > player.transform.position.x)
        {
            right = false;
        }
        else
        {
            right = true;
        }

        if (right == true)
        {
            me.SetDirection(1);
            transform.localScale = new Vector3(scaleCache * me.GetDirection(), transform.localScale.y, transform.localScale.z);
        }
        if (right == false)
        {
            me.SetDirection(-1);
            transform.localScale = new Vector3(scaleCache * me.GetDirection(), transform.localScale.y, transform.localScale.z);
        }
        
        velocity.x = speed * me.GetDirection();
        velocity.y = body.velocity.y;
        body.velocity = velocity;
        
    }

}

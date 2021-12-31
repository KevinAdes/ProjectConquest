using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public int speed;
    public Rigidbody2D body;
    Vector2 velocity;
    int direction;

    [HideInInspector]
    public bool vulerable = true;
    Entity me;
    PlayerMovement Dracula;
    float scaleCache;

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Entity>();
        Dracula = FindObjectOfType<PlayerMovement>();
        scaleCache = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        switch (me.detected)
        {
            case true:
                Run();
                break;
            case false:
                break;
        }
    }

    private void Run()
    {
        bool right;
        print(transform.position.x);
        if (transform.position.x > Dracula.transform.position.x)
        {
            right = false;
        }
        else
        {
            right = true;
        }

        if (right == true)
        {
            direction = 1;
            transform.localScale = new Vector3(scaleCache * direction, transform.localScale.y, transform.localScale.z);
        }
        if (right == false)
        {
            direction = -1;
            transform.localScale = new Vector3(scaleCache * direction, transform.localScale.y, transform.localScale.z);
        }
        
        velocity.x = speed * direction;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
        
    }

}

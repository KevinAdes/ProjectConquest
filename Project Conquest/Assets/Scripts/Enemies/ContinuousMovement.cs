﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{

    [Header("Calculation Variables")]
    [SerializeField]
    float speed;

    Entity entity;
    Rigidbody2D body;
    float scaleCache;
    Vector2 moveVector = new Vector2(0,0);

    // Start is called before the first frame update
    void Awake()
    {
        scaleCache = transform.localScale.x;
        if (GetComponent<Entity>() != null)
        {
            entity = GetComponent<Entity>();
            speed = entity.GetSpeed();
            body = GetComponent<Rigidbody2D>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        Direction();
    }

    private void Direction()
    {
        transform.localScale = new Vector3(scaleCache * entity.GetDirection(), transform.localScale.y, transform.localScale.z);
        moveVector.x = speed * entity.GetDirection() * Time.deltaTime * 1.5f;
        body.velocity += moveVector;
    }


}

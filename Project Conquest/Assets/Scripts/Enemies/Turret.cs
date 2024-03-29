﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillsList;

public class Turret : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    
    [SerializeField]
    float fireRate;
    
    [SerializeField]
    float force;
    float shotSpacing = 0;

    GameManager manager;

    public Transform target;
    public bool detected = false;
    Vector2 direction = new Vector2(0, 0);
    PlayerMovement player;

    Entity me;

    void OnEnable()
    {
        me = GetComponent<Entity>();
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (detected)
        {
            case true:
                Seek();
                break;
            case false:
                break;
        }
    }

    private void Seek()
    {
        target = player.transform;
        Vector2 targetPosition = target.position;
        direction = targetPosition - (Vector2)transform.position;
        transform.up = -direction;

        if (Time.time > shotSpacing)
        {
            shotSpacing = Time.time + 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject BulletInst = Instantiate(projectile, transform.position, Quaternion.identity);
        BulletInst.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }
}  

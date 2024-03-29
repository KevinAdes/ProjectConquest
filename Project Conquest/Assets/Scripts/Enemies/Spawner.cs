﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawn;
    [SerializeField]
    float fireRate;
    float shotSpacing = 0;


    Interactable me;
    public void Start()
    {
        me = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (me.GetDetected())
        {
            case true:
                SpawnTimer();
                break;
            case false:
                break;
        }
    }

    private void SpawnTimer()
    {
        if (Time.time > shotSpacing)
        {
            shotSpacing = Time.time + 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject EntityInst = Instantiate(spawn, transform.position, Quaternion.identity, transform.parent);
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawn;
    
    [SerializeField]
    Vector3 position;
    
    [SerializeField]
    float fireRate;
    
    float shotSpacing = 0;

    // Update is called once per frame
    void Update()
    {
        SpawnTimer();
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
        GameObject EntityInst = Instantiate(spawn, position, Quaternion.identity);
        EntityInst.transform.parent = gameObject.transform;
        int RandomDir = Random.Range(-1, 1);
        if(RandomDir < 0)
        {
            RandomDir = -1;
        }
        if(RandomDir >= 0)
        {
            RandomDir = 1;
        }
        EntityInst.GetComponent<Entity>().SetDirection(RandomDir);
        EntityInst.transform.GetChild(0).transform.position = position;
    }
}


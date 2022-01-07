using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSpawner : MonoBehaviour
{
    public GameObject spawn;
    public Vector3 position;
    public float fireRate;
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
        print(transform.position);
        GameObject EntityInst = Instantiate(spawn, position, Quaternion.identity);
        EntityInst.transform.GetChild(0).transform.position = position;
    }
}


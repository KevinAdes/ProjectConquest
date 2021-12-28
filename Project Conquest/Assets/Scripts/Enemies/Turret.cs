using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectile;
    public float fireRate;
    public float force;
    float shotSpacing = 0;

    public float range;
    public Transform target;
    public bool detected = false;
    Vector2 direction = new Vector2(0, 0);
    PlayerMovement Dracula;

    // Start is called before the first frame update
    void Start()
    {
        Dracula = FindObjectOfType<PlayerMovement>();
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
        target = Dracula.transform;
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

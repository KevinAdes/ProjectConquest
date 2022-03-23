using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkills", menuName = "ScriptableObjects/EnemySkills", order = 4)]
public class EnemySkills : ScriptableObject
{
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    float bulletPower;

    [SerializeField]
    GameObject minion;

    public delegate void func();


    public void SmallHop()
    {
        if (FindObjectOfType<PlayerMovement>().IsGrounded())
        {
            FindObjectOfType<Dracula>().gameObject.transform.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 4f);
        }
    }

    public void BigHop()
    {
        if (FindObjectOfType<PlayerMovement>().IsGrounded())
        {
            FindObjectOfType<Dracula>().gameObject.transform.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 8f);
        }
    }

    public void Fire()
    {
        bullet.GetComponent<PlayerBullet>().SetBullet(Mathf.RoundToInt(bulletPower * FindObjectOfType<Dracula>().damage));
        GameObject BulletInst = Instantiate(bullet, FindObjectOfType<PlayerMovement>().transform.position, Quaternion.identity);
        BulletInst.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * bulletSpeed);
    }

    public void Spawn()
    {

    }

    public void Slash()
    {
        if (FindObjectOfType<Dracula>().GetAttack() == false)
        {
            FindObjectOfType<Dracula>().SetAttack(true);
            //me.speed = me.speed / 2;
            FindObjectOfType<Dracula>().animator.SetTrigger("Attack");
        }
    }
}

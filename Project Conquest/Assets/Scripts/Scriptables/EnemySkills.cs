using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkills : MonoBehaviour
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
        GameObject BulletInst = Instantiate(bullet, transform.position, Quaternion.identity);
        BulletInst.GetComponent<Rigidbody2D>().AddForce(Vector3.forward * bulletSpeed);
    }

    public void Spawn()
    {

    }
}

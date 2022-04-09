using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    int dmg;

    public void SetBullet(int i)
    {
        dmg = i;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 1:
                collision.gameObject.GetComponent<DamageSystem>().TakeDamage(collision.gameObject.GetComponent<Rigidbody2D>(), transform.position - collision.transform.position + Vector3.up * 0.33f, collision.gameObject.GetComponent<DamageSystem>().DamageCalculator(dmg, collision.gameObject.GetComponent<DamageSystem>().GetEntity().GetDefense()));
                Destroy(gameObject);
                break;
            case 8:
                Destroy(gameObject);
                break;
            case 9:
                Destroy(gameObject);
                break;
            case 14:
                Destroy(gameObject);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 13:
                collision.gameObject.GetComponent<DamageSystem>().TakeDamage(collision.gameObject.GetComponent<Rigidbody2D>(), transform.position - collision.transform.position + Vector3.up * 0.33f, dmg);
                Destroy(gameObject);
                break;
            case 8:
                Destroy(gameObject);
                break;
            case 0:
                Destroy(gameObject);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        print("im colliding at least");
        if (collision.transform.tag == "Attack")
        {
            print("check");
            Destroy(gameObject);
        }
    }
}

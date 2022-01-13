using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyManager;

public class EnemySkills : MonoBehaviour
{
    public void SmallHop()
    {
        FindObjectOfType<Dracula>().gameObject.transform.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 50f);
        print(FindObjectOfType<Dracula>());
        print(FindObjectOfType<Dracula>().gameObject.transform.GetComponent<Rigidbody2D>().velocity);
    }

    public void Fire()
    {

    }
}

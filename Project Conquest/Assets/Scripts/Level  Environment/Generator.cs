   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public DefenseSystem[] Powering;
    Entity me;

    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (me.health <= 0)
        {
            print("yes");
            PowerDown();
        }
    }

    void PowerDown()
    {
        for (int i = 0; i < Powering.Length; i++)
        {
            if (Powering[i] != null)
            {
                Powering[i].PowerDown();
            }
            


        }
    }
}

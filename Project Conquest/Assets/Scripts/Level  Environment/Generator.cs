using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] Powering;
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
                if (Powering[i].GetComponent<Turret>() != null)
                {
                    print("beep");
                    Powering[i].GetComponent<Turret>().enabled = false;
                    Powering[i].GetComponent<Animator>().SetTrigger("PowerDown");
                }
                if (Powering[i].GetComponent<Spawner>() != null)
                {
                    Powering[i].GetComponent<Spawner>().enabled = false;
                    Powering[i].GetComponent<Animator>().SetTrigger("PowerDown");
                }
                if (Powering[i].GetComponent<Chaser>() != null)
                {
                    Powering[i].GetComponent<Chaser>().enabled = false;
                    Powering[i].GetComponent<Animator>().SetTrigger("PowerDown");
                }
                if (Powering[i].GetComponent<Defender>() != null)
                {
                    Powering[i].GetComponent<Defender>().enabled = false;
                    Powering[i].GetComponent<Animator>().SetTrigger("PowerDown");
                }

                if (Powering[i].GetComponent<Wanderer>() != null)
                {
                    Powering[i].GetComponent<Wanderer>().enabled = false;
                    Powering[i].GetComponent<Animator>().SetTrigger("PowerDown");
                }

            }
            


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSystem : MonoBehaviour
{

    public GameObject[] Powering;

    public void PowerDown()
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
    public void PowerOn()
    {
        for (int i = 0; i < Powering.Length; i++)
        {
            print("hello");
            if (Powering[i] != null)
            {
                if (Powering[i].GetComponent<Turret>() != null)
                {
                    print("this should turn on the turret...");
                    Powering[i].GetComponent<Turret>().enabled = true;
                }
                if (Powering[i].GetComponent<Spawner>() != null)
                {
                    Powering[i].GetComponent<Spawner>().enabled = true;
                }
                if (Powering[i].GetComponent<Chaser>() != null)
                {
                    Powering[i].GetComponent<Chaser>().enabled = true;
                }
                if (Powering[i].GetComponent<Defender>() != null)
                {
                    Powering[i].GetComponent<Defender>().enabled = true;
                }

                if (Powering[i].GetComponent<Wanderer>() != null)
                {
                    Powering[i].GetComponent<Wanderer>().enabled = true;
                }

            }
        }
    }
}

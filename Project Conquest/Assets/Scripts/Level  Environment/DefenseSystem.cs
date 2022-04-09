using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSystem : MonoBehaviour
{
    bool powered = true;

    [SerializeField]
    GameObject[] Powering;

    public void Awake()
    {
        if(powered == false)
        {
            PowerDown();
        }
    }

    public void PowerDown()
    {
        powered = false;
        for (int i = 0; i < Powering.Length; i++)
        {
            if (Powering[i] != null)
            {
                if (Powering[i].GetComponent<Turret>() != null)
                {
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
        if (powered == true)
        {
            for (int i = 0; i < Powering.Length; i++)
            {
                if (Powering[i] != null)
                {
                    if (Powering[i].GetComponent<Turret>() != null)
                    {
                        Turret temp = Powering[i].GetComponent<Turret>();
                        temp.enabled = true;
                        temp.detected = true;
                        temp.target = FindObjectOfType<Dracula>().transform;
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
}

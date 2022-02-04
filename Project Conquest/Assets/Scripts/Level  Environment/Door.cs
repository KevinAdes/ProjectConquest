using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] triggerGroupOff;
    public GameObject[] triggerGroupOn;
    public bool vertical;
    public bool right;

    //will eventually be used to determine if we need to press z to open or if it opens automatcally
    public bool closed;

    bool active;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        active = true;
        StartCoroutine(OpenDoor());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        active = false;
        for (int i = 0; i < triggerGroupOff.Length; i++)
        {
            if (triggerGroupOff[i] != null)
            {

                if (vertical == false)
                {
                    if (right == false && collision.transform.position.x < transform.position.x)
                    {
                        triggerGroupOff[i].SetActive(true);
                    }
                    if (right == true && collision.transform.position.x > transform.position.x)
                    {
                        triggerGroupOff[i].SetActive(true);
                    }

                }
                if (vertical == true)
                {
                    if (collision.transform.position.y > transform.position.y)
                    {
                        triggerGroupOff[i].SetActive(true);
                    }
                }
            }
        }
        for (int i = 0; i < triggerGroupOn.Length; i++)
        {
            if (triggerGroupOn[i] != null)
            {
                if (vertical == false)
                {
                    if (right == false && collision.transform.position.x < transform.position.x)
                    {
                        triggerGroupOn[i].SetActive(false);
                    }
                    if (right == true && collision.transform.position.x > transform.position.x)
                    {
                        triggerGroupOn[i].SetActive(false);
                    }

                }
                if (vertical == true)
                {
                    if (collision.transform.position.y > transform.position.y)
                    {
                        triggerGroupOn[i].SetActive(false);
                    }
                }

            }
            
        }

    }
    IEnumerator OpenDoor()
    {
        while (active)
        {
            if (!active) { break; }
            if (Input.GetAxis("Fire1") != 0)
            {

                for (int i = 0; i < triggerGroupOff.Length; i++)
                {
                    if (triggerGroupOff[i] != null)
                    {
                        triggerGroupOff[i].SetActive(false);
                    }
                }
                for (int i = 0; i < triggerGroupOn.Length; i++)
                {
                    if (triggerGroupOn[i] != null)
                    {
                        triggerGroupOn[i].SetActive(true);
                    }
                }
                break;
            }
            yield return null;
        }
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] triggerGroupOff;
    public GameObject[] triggerGroupOn;
    public bool vertical;

    bool active;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        active = true;
        while (active)
        {
            if (Input.GetAxis("Fire1") != 0)
            {

                for (int i = 0; i < triggerGroupOff.Length; i++)
                {
                    triggerGroupOff[i].SetActive(false);
                    print(triggerGroupOff[i].name);
                }
                for (int i = 0; i < triggerGroupOn.Length; i++)
                {
                    triggerGroupOn[i].SetActive(true);
                }
            }

        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        active = false;
        for (int i = 0; i < triggerGroupOff.Length; i++)
        {
            if (vertical == false)
            {
                if (transform.position.x < triggerGroupOff[i].transform.position.x && collision.transform.position.x < transform.position.x)
                {
                    triggerGroupOff[i].SetActive(true);
                }
                if (transform.position.x > triggerGroupOff[i].transform.position.x && collision.transform.position.x > transform.position.x)
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
        for (int i = 0; i < triggerGroupOn.Length; i++)
        {
            if (vertical == false)
            {
                if (transform.position.x < triggerGroupOn[i].transform.position.x && collision.transform.position.x < transform.position.x)
                {
                    triggerGroupOn[i].SetActive(false);
                }
                if (transform.position.x > triggerGroupOn[i].transform.position.x && collision.transform.position.x > transform.position.x)
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

    private void Down()
    {
        
    }

    private void Up()
    {

    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject triggerForeground;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        triggerForeground.SetActive(false);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(transform.position.x < triggerForeground.transform.position.x && collision.transform.position.x < transform.position.x)
        {
            triggerForeground.SetActive(true);
        }
        if (transform.position.x > triggerForeground.transform.position.x && collision.transform.position.x > transform.position.x)
        {
            triggerForeground.SetActive(true);
        }
    }
}


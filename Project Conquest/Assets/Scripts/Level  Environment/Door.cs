using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    Item key;

    GameManager manager;

    int ID;
    public GameObject[] triggerGroupOff;
    public GameObject[] triggerGroupOn;
    public bool vertical;
    public bool right;

    [SerializeField]
    bool closed;

    bool active;

    public void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        active = true;
        if (collision.gameObject.layer == 13)
        {            
            StartCoroutine(OpenDoor(collision));
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            active = false;
            for (int i = 0; i < triggerGroupOff.Length; i++)
            {
                if (triggerGroupOff[i] != null && triggerGroupOff[i].activeSelf == true)
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

        if (collision.gameObject.layer == 10)
        {
            if (right == false && collision.transform.position.x < transform.position.x)
            {
                StartCoroutine(MoveGuy(collision));
            }
            if (right == true && collision.transform.position.x > transform.position.x)
            {
                StartCoroutine(MoveGuy(collision));
            }
        }
    }
    IEnumerator MoveGuy(Collider2D collision)
    {
        yield return new WaitForSeconds(.2f);
        collision.gameObject.transform.parent.transform.parent = FindObjectOfType<ParentalRedirect>(true).transform;
    }
    IEnumerator OpenDoor(Collider2D collision)
    {
        while (active)
        {
            if (!active) { break; }
            if (Input.GetAxis("Submit") != 0)
            {
                if(closed == false)
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

                if(closed == true)
                {
                    for (int i = 0; i < collision.GetComponent<Dracula>().GetInventory().Container.Count; i++)
                    {
                        if(collision.GetComponent<Dracula>().GetInventory().Container[i].item == key)
                        {
                            collision.GetComponent<Dracula>().GetInventory().RemoveItem(key);
                            manager.MarkOpened(ID, SceneManager.GetActiveScene().name);
                            closed = false;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    public void SetID(int i)
    {
        ID = i;
    }
    public int GetID()
    {
        return ID;
    }
    //this set may be unnecessary
    public void SetClosed(bool b)
    {
        closed = b;
    }
    public bool GetClosed()
    {
        return closed;
    }
}



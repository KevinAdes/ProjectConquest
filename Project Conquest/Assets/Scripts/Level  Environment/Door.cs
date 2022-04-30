using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    Item key;

    GameManager manager;

    int ID;
    [SerializeField]
    GameObject[] triggerGroupOff;

    [SerializeField]
    GameObject[] triggerGroupOn;

    [SerializeField]
    bool vertical;

    [SerializeField]
    bool right;

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
            FindObjectOfType<Dracula>().ActivateAlert();
            StartCoroutine(OpenDoor(collision));
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            collision.GetComponent<Dracula>().DeactivateAlert();
            active = false;
            if (right == false && collision.transform.position.x < transform.position.x)
            {
                for (int i = 0; i < triggerGroupOff.Length; i++)
                {
                    if (triggerGroupOff[i] != null)
                    {
                        triggerGroupOff[i].SetActive(true);
                    }
                }
                for (int i = 0; i < triggerGroupOn.Length; i++)
                {
                    if (triggerGroupOn[i] != null)
                    {
                        triggerGroupOn[i].SetActive(false);
                    }
                }
            }
            if (right == true && collision.transform.position.x > transform.position.x)
            {
                for (int i = 0; i < triggerGroupOff.Length; i++)
                {
                    if (triggerGroupOff[i] != null)
                    {
                        triggerGroupOff[i].SetActive(true);
                    }
                }
                for (int i = 0; i < triggerGroupOn.Length; i++)
                {
                    if (triggerGroupOn[i] != null)
                    {
                        triggerGroupOn[i].SetActive(false);
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
                collision.GetComponent<Dracula>().DeactivateAlert();
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

                if(closed == true  && key != null)
                {
                    for (int i = 0; i < collision.GetComponent<Dracula>().GetInventory().GetContainer().Count; i++)
                    {
                        if(collision.GetComponent<Dracula>().GetInventory().GetContainer()[i].item == key)
                        {
                            collision.GetComponent<Dracula>().GetInventory().RemoveItem(key);
                            manager.MarkOpened(ID, SceneManager.GetActiveScene().name);
                            closed = false;
                        }
                    }
                    GetComponent<CutsceneManager>().ExecuteCutscene();
                    FindObjectOfType<GameManager>().GetFlags().GetType().GetField("LockedDoor").SetValue(FindObjectOfType<GameManager>().GetFlags(), false);
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



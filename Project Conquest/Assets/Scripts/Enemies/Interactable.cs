using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    int ID;
    [SerializeField]
    int cash;
    [SerializeField]
    bool destructable;

    bool detected = false;

    GameManager manager;

    PauseControl control;

    LevelData Data;

    [SerializeField]
    GameItem[] drops;

    [SerializeField]
    UnityEvent Interaction;

    public void DoAction()
    {
        if (Interaction != null)
        {
            if (GetComponent<RoboGuy>() != null && GetComponent<RoboGuy>().GetAlerted() == true)
            {
                return;
            }
            Interaction?.Invoke();
            AdditionalAction();
        }
    }

    public void OnDestroy()
    {
        foreach (GameItem item in drops)
        {
            GameObject itemInst = Instantiate(item.gameObject, transform.position, Quaternion.identity);
            itemInst.transform.parent = this.transform.parent;
        }
        if (FindObjectOfType<InteractionInstigation>() != null && FindObjectOfType<InteractionInstigation>().GetInteractables().Contains(this))
        {
            FindObjectOfType<InteractionInstigation>().GetInteractables().Remove(this);
        }
    }
    //if the object being interacted on needs to be frozen, changed, etc
    public void AdditionalAction()
    {
        if (gameObject.GetComponent<RoboGuy>() != null)
        {
            gameObject.GetComponent<RoboGuy>().Freeze();
        }
    }

    public void undoAdditionalAction()
    {

        if (gameObject.GetComponent<RoboGuy>() != null)
        {
            gameObject.GetComponent<RoboGuy>().UnFreeze();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }
        if (control == null)
        {
            control = FindObjectOfType<PauseControl>();
        }
    }

    public void Death()
    {
        if (cash != 0)
        {
            control.AddCash(cash);
            cash = 0;
        }
        if (destructable == true)
        {

            if (GetComponent<Generator>() != null)
            {
                GetComponent<Generator>().PowerDown();
            }
            manager.markDestroyed(ID, SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
    }

    //Getters and Setters
    public int GetID()
    {
        return ID;
    }

    public void SetID(int i)
    {
        ID = i;
    }

    public bool GetDetected()
    {
        return detected;
    }
    public void SetDetected(bool b)
    {
        detected = b;
    }

    public UnityEvent GetInteraction()
    {
        return Interaction;
    }
}

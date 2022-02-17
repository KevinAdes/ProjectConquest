using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public int ID;
    public int cash;
    public bool destructable;

    [HideInInspector]
    public bool detected = false;

    GameManager manager;

    PauseControl control;

    LevelData Data;

    [SerializeField]
    public UnityEvent Interaction;

    public void DoAction()
    {
        if (Interaction != null)
        {
            Interaction?.Invoke();
            AdditionalAction();
        }
    }

    //if the object being interacted on needs to be frozen, changed, etc
    public void AdditionalAction()
    {
        if (gameObject.GetComponent<RoboGuy>() != false)
        {
            gameObject.GetComponent<RoboGuy>().Freeze();
        }
    }

    public void undoAdditionalAction()
    {

        if (gameObject.GetComponent<RoboGuy>() != false)
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
        if (destructable == true)
        {

            if (GetComponent<Generator>() != null)
            {
                GetComponent<Generator>().PowerDown();
            }
            manager.markDestroyed(ID, SceneManager.GetActiveScene().name);
            if (cash != 0)
            {
                control.AddCash(cash);
                cash = 0;
            }
            //Destroy(gameObject);
        }
    }
}

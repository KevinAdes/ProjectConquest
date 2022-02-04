using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int ID;
    public int cash;

    [HideInInspector]
    public bool detected = false;

    GameManager manager;

    PauseControl control;

    LevelData Data;
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

    // Update is called once per frame
    void Update()
    {
        Data = (LevelData)manager.table.Levels[SceneManager.GetActiveScene().name];
        print(Data.Interactables[ID].dead);
    }
    public void Death()
    {
        if(GetComponent<Generator>() != null)
        {
            GetComponent<Generator>().PowerDown();
        }
        manager.markDestroyed(ID, SceneManager.GetActiveScene().name);
        if (cash != 0)
        {
            control.AddCash(cash);
        }
        //Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseControl : MonoBehaviour
{
    //buttons
    public GameObject focusButton;
    public GameObject itemsTab;

    //texts
    public Text Power;
    public Text Defense;
    public Text Speed;
    public Text Health;
    public Text Experience;

    //screens
    public GameObject pauseScreen;

    //extra
    public PlayerMovement Dracula;
    bool isAxisInUse = false;
    bool pause = false;

    public void Calibrate()
    {
        Power.text = "POWER " + Dracula.damage;
        Defense.text = "DEFENSE " + Dracula.defense;
        Speed.text = "SPEED " + Dracula.speed;
        Health.text = "HEALTH " + Dracula.maxHealth;
        Experience.text = Dracula.blood.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    void Pause()
    {
        if (Input.GetAxisRaw("Pause") != 0)
        {
            if(isAxisInUse == false)
            {
                isAxisInUse = true;
                if (pause)
                {
                    pause = false;
                    pauseScreen.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    pause = true;
                    pauseScreen.SetActive(true);
                    Time.timeScale = 0;

                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(focusButton);
                    Calibrate();
                }
            }
        }
        if (Input.GetAxisRaw("Pause") == 0)
        {
            isAxisInUse = false;
        }
    }

    public void Unpause()
    { 
        pause = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseControl : MonoBehaviour
{
    public PlayerData playerData;
    public GameManager manager;
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
    Dracula dracula;
    bool isAxisInUse = false;
    bool pause = false;


    public void Awake()
    {  
        manager = FindObjectOfType<GameManager>();
        playerData = manager.playerData;
        dracula = FindObjectOfType<Dracula>();
        Calibrate();
    }

    void Update()
    {
        Pause();
    }

    //Pause Functions

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

    //upgrade functions

    public void UpgradeDamage()
    {
        print("call");
        if (playerData.blood > 0)
        {
            playerData.blood -= 1;
            playerData.damage += 1;
        }
        Calibrate();

    }

    public void UpgradeDefense()
    {
        if (playerData.blood > 0)
        {
            playerData.blood -= 1;
            playerData.defense += .1f;
        }
        Calibrate();

    }

    public void UpgradeSpeed()
    {
        if (playerData.blood > 0)
        {
            playerData.blood -= 1;
            playerData.speed += 1;
            //speed cap may need to be upgraded by a higher amount
            playerData.speedCap += 1;
        }
        Calibrate();

    }

    public void UpgradeHealth()
    {
        if (playerData.blood > 0)
        {
            playerData.blood -= 1;
            playerData.maxHealth += 1;
        }
        Calibrate();
    }

    public void Calibrate()
    {
        Power.text = "POWER " + playerData.damage;
        Defense.text = "DEFENSE " + playerData.defense;
        Speed.text = "SPEED " + playerData.speed;
        Health.text = "HEALTH " + playerData.maxHealth;
        Experience.text = playerData.blood.ToString();

        if (dracula != null)
        {
            dracula.maxHealth = playerData.maxHealth;
            dracula.damage = playerData.damage;
            dracula.defense = playerData.defense;
            dracula.attackModifier = playerData.attackModifier;

            dracula.speed = playerData.speed;
            dracula.speedCache = dracula.speed;
            dracula.speedCap = playerData.speedCap;
            dracula.speedCapCache = dracula.speedCap;
            dracula.jump = playerData.jump;
        }

    }

}
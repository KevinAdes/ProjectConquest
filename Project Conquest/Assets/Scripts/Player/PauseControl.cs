using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static EnemyManager;

public class PauseControl : MonoBehaviour
{
    public PlayerData playerData;
    public GameManager manager;
    public GameObject mainTab;
    public GameObject itemsTab;
    public GameObject equipmentTab;
    public GameObject upgradesTab;
    public GameObject journalTab;
    public GameObject optionsTab;

    //buttons
    public GameObject focusButton;
    public GameObject itemsButton;
    public GameObject equipmentButton;
    public GameObject upgradesButton;
    public GameObject journalButton;
    public GameObject optionsButton;
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
    public GameObject upgradesButtonListContent;
    bool isAxisInUse = false;
    bool pause = false;
    public List<func> functionHolder = new List<func>();


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

    public void Return()
    {
        itemsTab.SetActive(false);
        //equipmentTab.SetActive(false);
        upgradesTab.SetActive(false);
        //journalTab.SetActive(false);
        //optionsTab.SetActive(false);
        mainTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(focusButton);
    }

    public void Items()
    {
        mainTab.SetActive(false);
        itemsTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(itemsButton);
    }

    public void Equipment()
    {
        mainTab.SetActive(false);
        equipmentTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(equipmentButton);
    }

    public void Uprgrades()
    {
        mainTab.SetActive(false);
        upgradesTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(upgradesButton);
        for (int i = 0; i < upgradesButtonListContent.gameObject.transform.childCount; i++)
        {
            Destroy(upgradesButtonListContent.transform.GetChild(i));
        }
        List<func> funcs;
        foreach (string name in manager.enemies.Enemies.Keys)
        {
            Button enemyButton = new Button;
            funcs = (List<func>)manager.enemies.Enemies[name];
            dracula.checking = funcs[0];
        }
    }

    public void Journal()
    {
        mainTab.SetActive(false);
        journalTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(journalButton);
    }

    public void Options()
    {
        mainTab.SetActive(false);
        optionsTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void Save()
    {
        //Save player data
    }

    public void BackToMain()
    {
        //brings player back to main menu
    }

    //upgrade functions
    //TO BE MOVED TO A DIFFERENT SCRIPT
    //THIS FILE SHOULD BE FOR PAUSE FUNCTIONS ONLU
    //THESE SHOULD BE MOVED TO A DRACULA RELATED SCRIPT
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

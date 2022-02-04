using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static EnemyManager;
using System;

public class PauseControl : MonoBehaviour
{
    public PlayerData playerData;
    public GameManager manager;

    //tabs
    public GameObject mainTab;
    public GameObject itemsTab;
    public GameObject equipmentTab;
    public GameObject upgradesTab;
    public GameObject journalTab;
    public GameObject optionsTab;

    //buttons
    public GameObject defaultButton;
    public GameObject displayButton;
    public GameObject focusButton;
    public GameObject itemsButton;
    public GameObject equipmentButton;
    public GameObject upgradesButton;
    public GameObject journalButton;
    public GameObject optionsButton;

    Button temp;
    
    //texts
    public Text Power;
    public Text Defense;
    public Text Speed;
    public Text Health;
    public Text Experience;

    [SerializeField]
    public GameObject upgradesButtonListContent;
    public GameObject upgradesButtonListContentContent;

    //screens
    public GameObject pauseScreen;

    //extra
    Dracula dracula;
    bool isAxisInUse = false;
    bool pause = false;
    public List<func> functionHolder = new List<func>();
    XButton xButton;

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
                    Unpause();
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
        itemsTab.SetActive(false);
        //equipmentTab.SetActive(false);
        upgradesTab.SetActive(false);
        //journalTab.SetActive(false);
        //optionsTab.SetActive(false);
        mainTab.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(focusButton);
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
            upgradesButtonListContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (string name in manager.enemies.Enemies.Keys)
        {
            GameObject newButton = Instantiate(defaultButton);
            newButton.transform.SetParent(upgradesButtonListContent.transform);
            newButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
            newButton.GetComponent<XButton>().scroller = true;
            newButton.GetComponent<XButton>().Init(name);
            newButton.GetComponent<XButton>().control = this;
            if (temp == null)
            {
                newButton.GetComponent<XButton>().InitAbove(upgradesButton.GetComponent<Button>());
            }
            else
            {
                temp.GetComponent<XButton>().InitBelow(newButton.GetComponent<Button>());
                newButton.GetComponent<XButton>().InitAbove(temp);
            }
            temp = newButton.GetComponent<Button>();
        }

    }

    public void DisplayUpgrades(string name)
    {

        for (int i = 0; i < upgradesButtonListContentContent.gameObject.transform.childCount; i++)
        {
            upgradesButtonListContentContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (func skill in (List<func>)manager.enemies.Enemies[name])
        {
            GameObject newButton = Instantiate(displayButton);
            newButton.transform.SetParent(upgradesButtonListContentContent.transform);
            newButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
            xButton = newButton.GetComponent<XButton>();
            xButton.scroller = false;
            xButton.control = this;
            xButton.skill = skill;
            xButton.SetNameAndText(skill.Method.Name, "eventually i will need to figure out where to store all the text");

        }
    }

    public void GetSkillName(Action method)
    {

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

    public void AddCash(int gains)
    {
        playerData.cash += gains;
        print(playerData.cash);
    }

    public void MinCash(int loss)
    {
        playerData.cash -= loss;
    }

    //upgrade functions
    //add an  int paramater and a switch case to  handle  which  button the skill is getting assigned to
    public void AssignDracula(func skill)
    {
        dracula.process1 = skill;
    }

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

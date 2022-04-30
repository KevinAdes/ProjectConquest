using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static SkillsList;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{
    [SerializeField]
    PlayerData playerData;

    GameManager manager;

    //tabs
    [SerializeField]
    GameObject mainTab;
    [SerializeField]
    GameObject itemsTab;
    [SerializeField]
    GameObject equipmentTab;

    UpgradesTab upgradesTab;

    //These should each be given their own scripts once they are built
    GameObject journalTab;
    GameObject optionsTab;

    //buttons
    [SerializeField]
    GameObject defaultButton;
    [SerializeField]
    GameObject displayButton;
    [SerializeField]
    GameObject focusButton;
    [SerializeField]
    GameObject itemsButton;
    [SerializeField]
    GameObject equipmentButton;
    [SerializeField]
    GameObject upgradesButton;
    [SerializeField]
    GameObject journalButton;
    [SerializeField]
    GameObject optionsButton;

    Button temp;

    //texts
    [SerializeField]
    Text Power;
    [SerializeField]
    Text Defense;
    [SerializeField]
    Text Speed;
    [SerializeField]
    Text Health;
    [SerializeField]
    Text Experience;
    [SerializeField]
    Text Cash;

    [SerializeField]
    GameObject upgradesButtonListContent;
    [SerializeField]
    GameObject upgradesButtonListContentContent;

    //screens
    [SerializeField]
    GameObject pauseScreen;

    //extra
    [SerializeField]
    Dracula dracula;
    [SerializeField]
    InventoryObject inventory;

    bool isAxisInUse = false;
    bool pause = false;

    public static PauseControl instance;

    EnemySkill heldSkill;

    [SerializeField]
    UnityEvent process1;
    UnityEvent process2;
    UnityEvent process3;
    UnityEvent process4;


    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        upgradesTab = FindObjectOfType<UpgradesTab>(true);
        manager = FindObjectOfType<GameManager>();
        playerData = manager.GetPlayerData();
        inventory = playerData.GetInventory();

    }
    
    void Update()
    {
        if (dracula != null)
        {
            Pause();
        }
    }

    //Pause Functions

    void Pause()
    {
        if (dracula.GetState() == states.DEFAULT && Input.GetAxisRaw("Pause") != 0)
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
                    dracula.GetComponent<InteractionInstigation>().enabled = false;
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
        dracula.GetComponent<InteractionInstigation>().enabled = true;
        itemsTab.SetActive(false);
        //equipmentTab.SetActive(false);
        upgradesTab.gameObject.SetActive(false);
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
        upgradesTab.gameObject.SetActive(false);
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
        UpdateInventory();
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
        playerData.AddCash(gains);
    }

    //upgrade functions
    //add an  int paramater and a switch case to  handle  which  button the skill is getting assigned to
    public void UpgradeDamage()
    {
        print("call");
        if (playerData.GetBlood() > 0)
        {
            playerData.AddBlood(-1);
            playerData.SetDamage(playerData.GetDamage() + 1f) ;
        }
        Calibrate();

    }

    public void UpgradeDefense()
    {
        if (playerData.GetBlood() > 0)
        {
            playerData.AddBlood(-1);
            playerData.SetDefense(playerData.GetDefense() + 1f);
        }
        Calibrate();

    }

    public void UpgradeSpeed()
    {
        if (playerData.GetBlood() > 0)
        {
            playerData.AddBlood(-1);
            playerData.SetSpeed(playerData.GetSpeed() + 1f);
        }
        Calibrate();

    }

    public void UpgradeHealth()
    {
        if (playerData.GetBlood() > 0)
        {
            playerData.AddBlood(-1);
            playerData.SetMaxHealth(playerData.GetMaxHealth() + 1f);
        }
        Calibrate();
    }

    public void Calibrate()
    {
        Power.text = "POWER " + playerData.GetDamage();
        Defense.text = "DEFENSE " + playerData.GetDefense();
        Speed.text = "SPEED " + playerData.GetSpeed();
        Health.text = "HEALTH " + playerData.GetMaxHealth();
        Experience.text = "BLOOD " + playerData.GetBlood();
        Cash.text = "MONEY " + playerData.GetCash();

        if (dracula != null)
        {
            dracula.SetMaxHealth(playerData.GetMaxHealth());
            dracula.SetDamage(playerData.GetDamage());
            dracula.SetDefense(playerData.GetDefense());
            dracula.SetAttackModifier(playerData.GetAttackModifier());

            dracula.SetSpeed(playerData.GetSpeed());
            dracula.SetJump(playerData.GetJump());
        }

    }

    public void UpdateInventory()
    {
        GetComponentInChildren<DisplayInventory>(true).UpdateDisplay();
    }

    public void OnApplicationQuit()
    {
        inventory.GetContainer().Clear();
    }

    public void AssignDracula(EnemySkill skill, XButton button)
    {
        if (!skill.GetPurchased())
        {
            if(skill.GetPrice() <= playerData.GetBlood())
            {
                heldSkill = skill;
                upgradesTab.DisplayAssign();
                skill.SetPurchased(true);
                button.DeactivatePrice();
                playerData.AddBlood(-skill.GetPrice());

            }
        }
        else
        {
            heldSkill = skill;
            upgradesTab.DisplayAssign();
        }
    }

    //Getters and Setters
    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void SetDracula(Dracula newDracula)
    {
        dracula = newDracula;
    }

    public InventoryObject GetInventory()
    {
        return inventory;
    }

    public GameObject GetMainTab()
    {
        return mainTab;
    }

    public UpgradesTab GetUpgradesTab()
    {
        return upgradesTab;
    }

    //Process setters
    public void SetProcess1()
    {
        process1 = heldSkill.GetSkill();
        heldSkill = null;
    }
    public UnityEvent GetProcess1()
    {
        return process1;
    }

    public void SetProcess2()
    {
        process2 = heldSkill.GetSkill();
        heldSkill = null;
    }
    public UnityEvent GetProcess2()
    {
        return process2;
    }

    public void SetProcess3()
    {
        process3 = heldSkill.GetSkill();
        heldSkill = null;
    }
    public UnityEvent GetProcess3()
    {
        return process3;
    }

    public void SetProcess4()
    {
        process4 = heldSkill.GetSkill();
        heldSkill = null;
    }
    public UnityEvent GetProcess4()
    {
        return process4;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static SkillsList;
using TMPro;

public class XButton : MonoBehaviour
{
    public PauseControl control;
    Button button;

    [SerializeField]
    TextMeshProUGUI buttonName;
    [SerializeField]
    TextMeshProUGUI buttonPrice;
    [SerializeField]
    TextMeshProUGUI buttonDescription;


    public bool scroller;
    EnemySkill skill;

    public void Init(string s)
    {
        gameObject.name = s;
        button = GetComponent<Button>();
        buttonName.text = s;
    }

    public void LoadList()
    {
        if (scroller == true)
        {
            control.GetUpgradesTab().DisplayUpgrades(buttonName.text);
        }
    }


    public void DeactivatePrice()
    {
        buttonPrice.transform.gameObject.SetActive(false);
    }

    public void SetNameAndText(int price, string name, string description)
    {
        buttonPrice.text = price.ToString();
        buttonName.text = name;
        buttonDescription.text = description;
    }
    public void SetNameAndText(string name, string description)
    {
        buttonName.text = name;
        buttonDescription.text = description;
    }

    public void SetPlayerMove()
    {
        control.AssignDracula(skill, this);
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetFunc(EnemySkill s)
    {
        skill = s;
    }
}

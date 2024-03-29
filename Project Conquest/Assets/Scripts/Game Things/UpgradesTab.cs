using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//The purpose of this script is to hold references for the pause controller script, reducing clutter in the editor
public class UpgradesTab : MonoBehaviour
{

    [SerializeField]
    PauseControl control;

    [SerializeField]
    GameObject button;

    [SerializeField]
    GameObject enemyNames;

    [SerializeField]
    GameObject upgradeNames;

    [SerializeField]
    GameObject buttonAssignPanel;

    [SerializeField]
    GameObject defaultButton;

    [SerializeField]
    GameObject displayButton;

    [SerializeField]
    GameObject assignButton;

    [SerializeField]
    GameObject returnButton;

    XButton xButton;

    Button temp;

    public void Display()
    {
        control.GetMainTab().SetActive(false);
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
        for (int i = 0; i < enemyNames.gameObject.transform.childCount; i++)
        {
            enemyNames.transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (string name in FindObjectOfType<GameManager>().GetEnemies().Enemies.Keys)
        {
            GameObject newButton = Instantiate(defaultButton, enemyNames.transform, false);
            newButton.GetComponent<XButton>().SetScroller(true);
            newButton.GetComponent<XButton>().Init(name);
            newButton.GetComponent<XButton>().SetControl(control);
            temp = newButton.GetComponent<Button>();
        }
    }


    public void DisplayUpgrades(string name)
    {
        for (int i = 0; i < upgradeNames.gameObject.transform.childCount; i++)
        {
            upgradeNames.transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (EnemySkill skill in (List<EnemySkill>)FindObjectOfType<GameManager>().GetEnemies().Enemies[name])
        {
            GameObject newButton = Instantiate(displayButton, upgradeNames.transform, false);
            xButton = newButton.GetComponent<XButton>();
            xButton.SetScroller(false);
            xButton.SetControl(control) ;
            xButton.SetFunc(skill);
            if (!skill.GetPurchased())
            {
                xButton.SetNameAndText(skill.GetPrice(), skill.GetSkill().GetPersistentMethodName(0).ToString(), skill.GetDescription());
            }
            else
            {
                xButton.DeactivatePrice();
                xButton.SetNameAndText(skill.GetSkill().GetPersistentMethodName(0).ToString(), skill.GetDescription());
            }
        }
    }

    public void DisplayAssign()
    {
        buttonAssignPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(assignButton);
    }

    public void HideAssign()
    {
        buttonAssignPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(returnButton);
    }
}

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
        print(FindObjectOfType<GameManager>().enemies.Enemies.Keys.Count);
        foreach (string name in FindObjectOfType<GameManager>().enemies.Enemies.Keys)
        {
            GameObject newButton = Instantiate(defaultButton, enemyNames.transform, false);
            newButton.GetComponent<XButton>().scroller = true;
            newButton.GetComponent<XButton>().Init(name);
            newButton.GetComponent<XButton>().control = control ;
            temp = newButton.GetComponent<Button>();
        }
    }


    public void DisplayUpgrades(string name)
    {
        for (int i = 0; i < upgradeNames.gameObject.transform.childCount; i++)
        {
            upgradeNames.transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (EnemySkill skill in (List<EnemySkill>)FindObjectOfType<GameManager>().enemies.Enemies[name])
        {
            GameObject newButton = Instantiate(displayButton, upgradeNames.transform, false);
            xButton = newButton.GetComponent<XButton>();
            xButton.scroller = false;
            xButton.control = control;
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

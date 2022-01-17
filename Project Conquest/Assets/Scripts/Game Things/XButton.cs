using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyManager;

public class XButton : MonoBehaviour
{
    public PauseControl control;
    Button button;
    Text text;
    public bool scroller;
    public func skill;
    public void Init(string name)
    {
        gameObject.name = name;
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        text.text = name;
    }

    public void LoadList()
    {
        if (scroller == true)
        {
            control.DisplayUpgrades(text.text);
        }
    }

    public void InitAbove(Button newButton)
    {
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = newButton;
        button.navigation = navigation;
    }
    public void InitBelow(Button newButton)
    {
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = newButton;
        button.navigation = navigation;
    }
    public void InitLeft(Button newButton)
    {
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnLeft = newButton;
        button.navigation = navigation;
    }
    public void InitRight(Button newButton)
    {
         Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnRight = newButton;
        button.navigation = navigation;
    }

    public void SetNameAndText(string name, string description)
    {
        text = transform.Find("Name").GetComponent<Text>();
        text.text = name;
        text = transform.Find("Description").GetComponent<Text>();
        text.text = description;
    }

    public void SetPlayerMove()
    {
        print("ITS HAPPENING");
        control.AssignDracula(skill);
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemySkill", menuName = "ScriptableObjects/EnemySkill", order = 4)]
public class EnemySkill : ScriptableObject
{
    [SerializeField]
    int cost;
    [SerializeField]
    bool purchased;
    [SerializeField]
    UnityEvent skill;


    [TextArea(15, 20)]
    [SerializeField]
    string description;

    public UnityEvent GetSkill()
    {
        return skill;
    }
    public string GetDescription()
    {
        return description;
    }

    public int GetPrice()
    {
        return cost;
    }

    public void SetPurchased(bool b)
    {
        purchased = b;
    }
    public bool GetPurchased()
    {
        return purchased;
    }
}

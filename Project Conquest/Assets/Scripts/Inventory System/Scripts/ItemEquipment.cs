using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory System/Items/Equipment")]
public class ItemEquipment : Item
{
    [SerializeField]
    int attackBonus;
    [SerializeField]
    int defenseBonus;

    public void Awake()
    {
        type = itemType.Equipment;
    }

    public int GetABonus()
    {
        return attackBonus;
    }
    public int GetDBonus()
    {
        return defenseBonus;
    }
}

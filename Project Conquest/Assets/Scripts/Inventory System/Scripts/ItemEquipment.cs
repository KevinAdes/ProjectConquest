using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory System/Items/Equipment")]
public class ItemEquipment : Item
{
    public int attackBonus;
    public int defenseBonus;
    public void Awake()
    {
        type = itemType.Equipment;
    }
}

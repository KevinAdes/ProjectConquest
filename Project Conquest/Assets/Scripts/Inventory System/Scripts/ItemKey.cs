using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory System/Items/Key")]
public class ItemKey : Item
{
    public void Awake()
    {
        type = itemType.Key;
    }

}
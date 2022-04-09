using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Inventory System/Items/Food")]
public class ItemFood : Item
{
    [SerializeField]
    int restoreValue;

    public void Awake()
    {
        type = itemType.Food;
    }

    public int GetRestore()
    {
        return restoreValue;
    }
}

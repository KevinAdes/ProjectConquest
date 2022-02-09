using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(Item item, int count)
    {
        bool hasItem = false;
        for(int  i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Container[i].addAmount(count);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(item, count));
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int count;
    public InventorySlot(Item _item, int _count)
    {
        item = _item;
        count = _count;
    }

    public void addAmount(int amount)
    {
        count += amount;
    }
}
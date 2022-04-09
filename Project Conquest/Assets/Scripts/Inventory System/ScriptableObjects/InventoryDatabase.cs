using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Database", menuName = "Inventory System/Items/Database")]
public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    Item[] items;


    Dictionary<Item, int> getID = new Dictionary<Item, int>();

    Dictionary<int, Item> getItem = new Dictionary<int, Item>();

    public void OnAfterDeserialize()
    {
        getID = new Dictionary<Item, int>();
        getItem = new Dictionary<int, Item>();
        for (int i = 0; i <items.Length; i++)
        {
            getID.Add(items[i], i);
            getItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }

    public Dictionary<Item, int> GetIDDict()
    {
        return getID;
    }

    public Dictionary<int, Item> GetItemDict()
    {
        return getItem;
    }
}

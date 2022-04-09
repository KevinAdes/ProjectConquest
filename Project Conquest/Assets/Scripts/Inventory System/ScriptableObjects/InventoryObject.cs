using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using System.ComponentModel;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    string savePath;
    [SerializeField]
    private InventoryDatabase database;
    [SerializeField]
    List<InventorySlot> container = new List<InventorySlot>();

    public void OnEnable()
    {
        database = (InventoryDatabase)Resources.Load("Database");
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == item)
            {
                container[i].RemoveAmount(1);
                if(container[i].count == 0)
                {
                    container.Remove(container[i]);
                }
                return;
            }
        }
    }

    public void AddItem(Item item, int count)
    {
        for(int  i = 0; i < container.Count; i++)
        {
            if (container[i].item == item)
            {
                container[i].addAmount(count);
                return;
            }
        }
        container.Add(new InventorySlot(database.GetIDDict()[item], item, count));
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < container.Count; i++)
        {
            container[i].item = database.GetItemDict()[container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }

    public List<InventorySlot> Copy()
    {
        List<InventorySlot> newContainer = new List<InventorySlot>();

        for (int i = 0; i < container.Count; i++)
        {
            newContainer.Add(new InventorySlot(container[i].ID, container[i].item, container[i].count));
        }
        return newContainer;
    }

    public List<InventorySlot> GetContainer()
    {
        return container;
    }

    public void SetContainer(List<InventorySlot> l)
    {
        container = l;
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int count;
    public InventorySlot(int _id, Item _item, int _count)
    {
        ID = _id;
        item = _item;
        count = _count;
    }

    public void RemoveAmount(int amount)
    {
        count -= amount;
    }

    public void addAmount(int amount)
    {
        count += amount;
    }

}
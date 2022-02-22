using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private InventoryDatabase database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void OnEnable()
    {
        database = (InventoryDatabase)Resources.Load("Database");
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Container[i].UpdateSlot(1);
                return;
            }
        }
    }

    public void AddItem(Item item, int count)
    {
        for(int  i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Container[i].addAmount(count);
                return;
            }
        }
        Container.Add(new InventorySlot(database.getID[item], item, count));
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
        for (int i = 0; i < Container.Count; i++)
        {
            Container[i].item = database.getItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
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

    public void UpdateSlot(int amount)
    {
        count -= amount;
        if(count == 0)
        {
            item = null;
        }
    }

    public void addAmount(int amount)
    {
        count += amount;
    }
}
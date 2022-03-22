using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField]
    InventoryObject inventory;

    [SerializeField]
    InventoryObject targetInventory;

    public int X_START;
    public int Y_START;
    public int X_SPACER;
    public int Y_SPACER;
    public int columns;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {

            var obj = Instantiate(inventory.Container[i].item.prefab, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].count.ToString("n0");
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACER * (i % columns)), Y_START + ( -Y_SPACER * (i / columns)), 0f);
    }

    public void UpdateDisplay()
    {
        //would like to do something more efficient than cross referencing every child with every item in the inventory, but for now this seems like the way to do it
        foreach (GameItem child in GetComponentsInChildren<GameItem>())
        {
            bool found = false;
            for (int i = 0; i < inventory.Container.Count; i++)
            {
                if(inventory.Container[i].item == child.item)
                {
                    found = true;
                }
            }
            if (!found)
            {
                Destroy(child.gameObject);
            }
        }
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].count.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].count.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }

    //Getters and setters

    public void SetInventory(InventoryObject IO)
    {
        inventory = IO;
    }
    public InventoryObject GetInventory()
    {
        return inventory;
    }
    public InventoryObject GetTarget()
    {
        return targetInventory;
    }


}

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

    [SerializeField]
    int X_START;
    [SerializeField]
    int Y_START;
    [SerializeField]
    int X_SPACER;
    [SerializeField]
    int Y_SPACER;
    [SerializeField]
    int columns;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.GetContainer().Count; i++)
        {

            var obj = Instantiate(inventory.GetContainer()[i].item.prefab, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetContainer()[i].count.ToString("n0");
            itemsDisplayed.Add(inventory.GetContainer()[i], obj);
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
            for (int i = 0; i < inventory.GetContainer().Count; i++)
            {
                if(inventory.GetContainer()[i].item == child.GetItem())
                {
                    found = true;
                }
            }
            if (!found)
            {
                Destroy(child.gameObject);
            }
        }
        for(int i = 0; i < inventory.GetContainer().Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.GetContainer()[i]))
            {
                itemsDisplayed[inventory.GetContainer()[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetContainer()[i].count.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.GetContainer()[i].item.prefab, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.GetContainer()[i].count.ToString("n0");
                itemsDisplayed.Add(inventory.GetContainer()[i], obj);
            }
        }
    }
    public void ClearInventory()
    {
        inventory = null;
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

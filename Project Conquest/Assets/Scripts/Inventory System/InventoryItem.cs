using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public InventoryObject playerInventory;
    GameItem thisItem;

    public void Awake()
    {
        thisItem = GetComponent<GameItem>();
    }

    public void OnHover()
    {
        //these may be replaceable with one getcomponent to check if its a shop
        if (GetComponentInParent<PauseControl>() != null)
        {
            //pass
        }
        if (GetComponentInParent<StorageUnit>() != null)
        {
            //pass
        }
        else
        {
            string priceDialogue;
            priceDialogue = thisItem.item.name + ": " + thisItem.item.price;
            GetComponentInParent<UIDialogueTextBoxController>().SetText(priceDialogue);
        }
    }

    public void onClick()
    {

        //if the object is in the players inventory
        if (GetComponentInParent<PauseControl>()!= null)
        {
            //use the item
        }
        if (GetComponentInParent<StorageUnit>() != null)
        {
            print(GetComponentInParent<DisplayInventory>().GetTarget());
            GetComponentInParent<DisplayInventory>().GetTarget().AddItem(GetComponent<GameItem>().item, 1);
            GetComponentInParent<DisplayInventory>().GetInventory().RemoveItem(GetComponent<GameItem>().item);
            StorageUnit storageUnit = GetComponentInParent<StorageUnit>();
            storageUnit.UpdateDisplay();
        }
        else
        {
            if(FindObjectOfType<PauseControl>().GetPlayerData().cash >= thisItem.item.price)
            {
                FindObjectOfType<PauseControl>().GetPlayerData().cash -= thisItem.item.price;
                playerInventory.AddItem(GetComponent<GameItem>().item, 1);
                GetComponentInParent<DisplayInventory>().GetInventory().RemoveItem(GetComponent<GameItem>().item);
                GetComponentInParent<DisplayInventory>().UpdateDisplay();
            }
        }
    }
}

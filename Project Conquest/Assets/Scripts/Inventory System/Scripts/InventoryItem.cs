using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    InventoryObject playerInventory;
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
            priceDialogue = thisItem.GetItem().name + ": " + thisItem.GetItem().price;
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
            GetComponentInParent<DisplayInventory>().GetTarget().AddItem(GetComponent<GameItem>().GetItem(), 1);
            GetComponentInParent<DisplayInventory>().GetInventory().RemoveItem(GetComponent<GameItem>().GetItem());
            StorageUnit storageUnit = GetComponentInParent<StorageUnit>();
            storageUnit.UpdateDisplay();
        }
        else
        {
            if(FindObjectOfType<PauseControl>().GetPlayerData().cash >= thisItem.GetItem().price)
            {
                FindObjectOfType<PauseControl>().GetPlayerData().cash -= thisItem.GetItem().price;
                playerInventory.AddItem(GetComponent<GameItem>().GetItem(), 1);
                GetComponentInParent<DisplayInventory>().GetInventory().RemoveItem(GetComponent<GameItem>().GetItem());
                GetComponentInParent<DisplayInventory>().UpdateDisplay();
            }
        }
    }
}

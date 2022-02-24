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
        if (GetComponentInParent<PauseControl>() != null)
        {
            //use the item
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
        else
        {
            if(FindObjectOfType<PauseControl>().playerData.cash >= thisItem.item.price)
            {
                FindObjectOfType<PauseControl>().playerData.cash -= thisItem.item.price;
                playerInventory.AddItem(GetComponent<GameItem>().item, 1);
                GetComponentInParent<DisplayInventory>().inventory.RemoveItem(GetComponent<GameItem>().item);
                GetComponentInParent<DisplayInventory>().UpdateDisplay();
            }
        }
    }
}

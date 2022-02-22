using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItem : MonoBehaviour
{
    public InventoryObject playerInventory;
    public void onClick()
    {

        //if the object is in the players inventory
        if (GetComponentInParent<PauseControl>()!= null)
        {
            //use the item
        }
        else
        {
            playerInventory.AddItem(GetComponent<GameItem>().item, 1);
            GetComponentInParent<DisplayInventory>().inventory.RemoveItem(GetComponent<GameItem>().item);
            GetComponentInParent<DisplayInventory>().UpdateDisplay();
        }

    }
}

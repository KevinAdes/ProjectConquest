using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    [SerializeField]
    Item item;

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item i)
    {
        item = i;
    }
}

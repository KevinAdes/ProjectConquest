using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Food,
    Equipment,
    Key,
    Default
}

public abstract class Item : ScriptableObject
{
    public GameObject prefab;
    public itemType type;
    [TextArea(15,20)]
    public string description;
    public int price;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Table", menuName = "ScriptableObjects/LevelTable", order = 1)]
public class LevelTable : ScriptableObject
{
    public Hashtable Levels = new Hashtable();
}

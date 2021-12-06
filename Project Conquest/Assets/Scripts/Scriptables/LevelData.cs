using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{

    public string levelID;

    public EnemyManager[] Entities;
    public GameObject[] Interactables;

}

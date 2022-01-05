using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{

    public string levelID;

    public Vector2 leftSpawn;
    public Vector2 rightSpawn;

    public EnemyManager[] Entities;
    public GameObject[] Interactables;

}

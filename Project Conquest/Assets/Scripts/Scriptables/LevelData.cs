using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{

    public string levelID;
    bool right = false;

    //Marks the level to determine spawn location. false = left, true = right.
    public void SetRight(bool b)
    {
        right = b;
    }
    public bool GetRight()
    {
        return right;
    }


    public Vector2 leftSpawn;
    public Vector2 rightSpawn;

    public EnemyManager[] Entities;
    public EnemyManager[] Interactables;
    //could eventually apply to non-door locked things
    public LockManager[] Doors;

}

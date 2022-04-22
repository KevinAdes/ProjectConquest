using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{

    string levelID;
    bool right = false;

    Vector2 leftSpawn;
    Vector2 rightSpawn;

    EnemyManager[] Entities;
    EnemyManager[] Interactables;
    //could eventually apply to non-door locked things
    LockManager[] Doors;

    public string GetID()
    {
        return levelID;
    }
    public void SetID(string s)
    {
        levelID = s;
    }
    //Marks the level to determine spawn location. false = left, true = right.
    public void SetRight(bool b)
    {
        right = b;
    }
    public bool GetRight()
    {
        return right;
    }

    public Vector2 GetLeftSpawn()
    {
        return leftSpawn;
    }

    public Vector2 GetRightSpawn()
    {
        return rightSpawn;
    }

    public void SetLeftSpawn(Vector2 v)
    {
        leftSpawn = v;
    }

    public void SetRightSpawn(Vector2 v)
    {
        rightSpawn = v;
    }

    public EnemyManager[] GetEntities()
    {
        return Entities;
    }

    public void SetEntities(EnemyManager[] eM)
    {
        Entities = eM;
    }

    public EnemyManager[] GetInteractables()
    {
        return Interactables;
    }

    public void SetInteractables(EnemyManager[] iM)
    {
        Interactables = iM;
    }

    public LockManager[] GetDoors()
    {
        return Doors;
    }

    public void SetDoors(LockManager[] dM)
    {
        Doors = dM;
    }
}

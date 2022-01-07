﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public bool icon;
    public string ID;
    GameManager manager;
    LevelData data;

    public Vector2[] levelSpawns;

    SpriteRenderer levelIcon;

    public void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        if (icon != true)
        {
            if (manager.table.Levels.Contains(SceneManager.GetActiveScene().name))
            {
                data = (LevelData)manager.table.Levels[ID];
                reinitializeEntities(data);
                print("huh");
                manager.CheckData(data);
            }
            else
            {
                data = ScriptableObject.CreateInstance<LevelData>();
                InitializeData(data);
                manager.CheckData(data);
            }
        }
        if (icon == true)
        {
            levelIcon = GetComponent<SpriteRenderer>();
        }
    }

    private void reinitializeEntities(LevelData data)
    {
        data.levelID = SceneManager.GetActiveScene().name;
        int count = 0;
        Entity[] entities = FindObjectsOfType<Entity>();
        foreach (Entity entity in entities)
        {
            if(entity.important == true)
            {
                EnemyManager guy = data.Entities[count];
                guy.guy = entity.gameObject;
                entity.ID = count;
                data.Entities[count] = guy;
                count++;


            }
        }

    }

    // Start is called before the first frame update
    private void InitializeData(LevelData data)
    {
        data.levelID = SceneManager.GetActiveScene().name;
        data.leftSpawn = levelSpawns[0];
        data.rightSpawn = levelSpawns[1];
        int count = 0;
        Entity[] entities = FindObjectsOfType<Entity>();
        data.Entities = new EnemyManager[entities.Length];
        foreach (Entity entity in entities)
        {
            if(entity.important == true)
            {
                EnemyManager guy = ScriptableObject.CreateInstance<EnemyManager>();
                guy.guy = entity.gameObject;
                guy.EnemyID = count;
                entity.ID = count;
                guy.dead = false;
                data.Entities[count] = guy;
                count++;

            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelIcon != null)
        {
            if (levelIcon.enabled == false)
            {
                levelIcon.enabled = true; 
                manager.LoadAlert(ID);
            }
        }
    }
}
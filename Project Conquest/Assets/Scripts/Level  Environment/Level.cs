using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EnemyManager;

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
        int count = 0;

        Entity[] entities = FindObjectsOfType<Entity>(true);
        List<Entity> temp = new List<Entity>(entities);
        int i = 0;
        while (i < temp.Count)
        {
            if (temp[i].important == false)
            {
                temp.Remove(temp[i]);
            }
            else
            {
                i++;
            }
        }
        entities = temp.ToArray();
        foreach (Entity entity in entities)
        {
            if(entity.important == true)
            {
                EnemyManager guy = data.Entities[count];
                guy.guy = entity.gameObject;
                guy.important = entity.important;
                entity.ID = count;
                guy.myName = entity.myName;
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
        Entity[] entities = FindObjectsOfType<Entity>(true);
        List<Entity> temp = new List<Entity>(entities);
        int i = 0;
        while (i < temp.Count)
        {
            if(temp[i].important == false)
            {
                temp.Remove(temp[i]);
            }
            else
            {
                i++;
            }
        }
        entities = temp.ToArray();
        data.Entities = new EnemyManager[entities.Length];
        foreach (Entity entity in entities)
        {
            if(entity.important == true)
            {
                print("i have found an important entity");
                EnemyManager guy = ScriptableObject.CreateInstance<EnemyManager>();
                guy.guy = entity.gameObject;
                guy.EnemyID = count;
                entity.ID = count;
                guy.myName = entity.myName;
                guy.skills = entity.skills;
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
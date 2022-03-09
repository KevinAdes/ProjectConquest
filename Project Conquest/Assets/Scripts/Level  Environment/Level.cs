using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            ID = SceneManager.GetActiveScene().name;
            if (manager.table.Levels.Contains(ID))
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

    private void InitializeData(LevelData data)
    {
        data.levelID = SceneManager.GetActiveScene().name;
        data.leftSpawn = levelSpawns[0];
        data.rightSpawn = levelSpawns[1];
        int count = 0;
        Entity[] entities = FindObjectsOfType<Entity>(true);
        Interactable[] interactables = FindObjectsOfType<Interactable>(true);
        Door[] doors = FindObjectsOfType<Door>(true);
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
        data.Interactables = new EnemyManager[interactables.Length];
        data.Doors = new LockManager[doors.Length];

        foreach (Entity entity in entities)
        {
            if(entity.important == true)
            {
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
        count = 0;
        foreach (Interactable interactable in interactables)
        {
            EnemyManager inter = ScriptableObject.CreateInstance<EnemyManager>();
            inter.guy = interactable.gameObject;
            inter.EnemyID = count;
            interactable.ID = count;
            inter.dead = false;
            data.Interactables[count] = inter;
            count++;
        }
        count = 0;
        foreach (Door door in doors)
        {
            LockManager locked = ScriptableObject.CreateInstance<LockManager>();
            locked.SetGuy(door.gameObject);
            locked.SetID(count);
            door.SetID(count);
            locked.SetClosed(door.GetClosed());
            data.Doors[count] = locked;
            count++;
        }
        //in the event that I add non door lockables, simply add another for loop without resetting count to 0.
    }

    private void reinitializeEntities(LevelData data)
    {
        int count = 0;
        Entity[] entities = FindObjectsOfType<Entity>(true);
        Interactable[] interactables = FindObjectsOfType<Interactable>(true);
        Door[] doors = FindObjectsOfType<Door>(true);
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
        //TODO a lot of the code in each for loop may be removeable... test this
        entities = temp.ToArray();
        foreach (Entity entity in entities)
        {
            if (entity.important == true)
            {
                EnemyManager guy = data.Entities[count];
                guy.guy = entity.gameObject;
                //
                guy.important = entity.important;
                guy.myName = entity.myName;
                //
                entity.ID = count;
                data.Entities[count] = guy;

                count++;
            }
        }
        count = 0;
        foreach (Interactable interactable in interactables)
        {
            EnemyManager inter = data.Interactables[count];
            inter.guy = interactable.gameObject;
            //
            inter.EnemyID = count;
            interactable.ID = count;
            //
            data.Interactables[count] = inter;
            count++;
        }
        count = 0;
        foreach (Door door in doors)
        {
            LockManager locked = data.Doors[count];
            locked.SetGuy(door.gameObject);
            //
            locked.SetID(count);
            //
            door.SetID(count);
            count++;
        }
        //in the event that I add non door lockables, simply add another for loop without resetting count to 0.
    }

    public LevelData GetData()
    {
        return data;
    }
}
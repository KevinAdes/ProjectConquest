using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SkillsList;

public class Level : MonoBehaviour
{
    [SerializeField]
    bool icon;
    [SerializeField]
    string ID;
    GameManager manager;
    LevelData data;

    [SerializeField]
    Vector2[] levelSpawns;

    SpriteRenderer levelIcon;

    public void Start()
    { 
        if(icon != true)
        {
            StartCoroutine(ManagerFinder());
        }
        if (icon == true)
        {
            levelIcon = GetComponent<SpriteRenderer>();
        }
    }

    IEnumerator ManagerFinder()
    {
        yield return new WaitForSeconds(.2f);
        manager = FindObjectOfType<GameManager>();
        ID = SceneManager.GetActiveScene().name;
        if (manager.GetTable().Levels.Contains(ID))
        {
            data = (LevelData)manager.GetTable().Levels[ID];
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
        data.SetID(SceneManager.GetActiveScene().name);
        data.SetLeftSpawn(levelSpawns[0]);
        data.SetRightSpawn(levelSpawns[1]);
        int count = 0;
        Entity[] entities = FindObjectsOfType<Entity>(true);
        Interactable[] interactables = FindObjectsOfType<Interactable>(true);
        Door[] doors = FindObjectsOfType<Door>(true);
        List<Entity> temp = new List<Entity>(entities);
        int i = 0;
        while (i < temp.Count)
        {
            if(temp[i].GetImportant() == false)
            {
                temp.Remove(temp[i]);
            }
            else
            {
                i++;
            }
        }
        entities = temp.ToArray();
        EnemyManager[] tempE = new EnemyManager[entities.Length];
        EnemyManager[] tempI = new EnemyManager[interactables.Length];
        LockManager[] tempD = new LockManager[doors.Length];

        foreach (Entity entity in entities)
        {
            if(entity.GetImportant() == true)
            {
                EnemyManager guy = ScriptableObject.CreateInstance<EnemyManager>();
                if(entity.GetSkills().Length > 0)
                {
                    foreach (EnemySkill skill in entity.GetSkills())
                    {
                        EnemySkill newSkill = Instantiate(skill);
                        guy.AddSkill(newSkill);
                    }
                }
                guy.SetGuy(entity.gameObject);
                guy.SetID(count);
                entity.SetID(count);
                guy.SetMyName(entity.GetName());
                guy.SetDead(false);
                tempE[count] = guy;
                count++;

            }
        }
        data.SetEntities(tempE);
        count = 0;
        foreach (Interactable interactable in interactables)
        {
            EnemyManager inter = ScriptableObject.CreateInstance<EnemyManager>();
            inter.SetGuy(interactable.gameObject);
            inter.SetID(count);
            interactable.SetID(count);
            inter.SetDead(false);
            tempI[count] = inter;
            count++;
        }
        data.SetInteractables(tempI);
        count = 0;
        foreach (Door door in doors)
        {
            LockManager locked = ScriptableObject.CreateInstance<LockManager>();
            locked.SetGuy(door.gameObject);
            locked.SetID(count);
            door.SetID(count);
            locked.SetClosed(door.GetClosed());
            tempD[count] = locked;
            count++;
        }
        data.SetDoors(tempD);
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
            if (temp[i].GetImportant() == false)
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
            if (entity.GetImportant() == true)
            {
                EnemyManager guy = data.GetEntities()[count];
                guy.SetGuy(entity.gameObject);
                //
                guy.SetImportant(entity.GetImportant());
                guy.SetMyName(entity.GetName());
                //
                entity.SetID(count);
                data.GetEntities()[count] = guy;

                count++;
            }
        }
        count = 0;
        foreach (Interactable interactable in interactables)
        {
            EnemyManager inter = data.GetInteractables()[count];
            inter.SetGuy(interactable.gameObject);
            //
            inter.SetID(count);
            interactable.SetID(count);
            //
            data.GetInteractables()[count] = inter;
            count++;
        }
        count = 0;
        foreach (Door door in doors)
        {
            LockManager locked = data.GetDoors()[count];
            locked.SetGuy(door.gameObject);
            //
            locked.SetID(count);
            //
            door.SetID(count);
            print("if something fucky is happening with a door it might be this");
            data.GetDoors()[count] = locked;
            count++;
        }
        //in the event that I add non door lockables, simply add another for loop without resetting count to 0.
    }

    public LevelData GetData()
    {
        return data;
    }

    public string GetID()
    {
        return ID;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public EnemyDictionary enemies;
    public PlayerData playerData;
    public LevelTable table;
    public MapFogTable mapFogTable;
    public GameObject LoadHider;
    public GameObject alertBox;
    LevelData temp;

    public EnemySkills skillsList;

    Vector3 mapulaTransform;
    public MapMovement Mapula;

    public Vector2 playerLevelTransform;

    Animator animator;

    public static GameManager instance;

    string target;

    bool right;
    bool cloudChecking = false;

    public void Update()
    {

        print(table.Levels.Count);
    }

    private void Awake()
    {
        //playerData = ScriptableObject.CreateInstance<PlayerData>();
        enemies = ScriptableObject.CreateInstance<EnemyDictionary>();
        table = ScriptableObject.CreateInstance<LevelTable>();
        mapFogTable = ScriptableObject.CreateInstance<MapFogTable>();
        animator = GetComponent<Animator>();
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    IEnumerator reload()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string ID)
    {
        target = ID;
        animator.SetTrigger("Show");
    }

    public void LoadLevel(string ID, Vector3 Mapula)
    {
        target = ID;
        mapulaTransform = Mapula;
        animator.SetTrigger("Show");
    }

    public void LoadLevelANIMATIONEVENT()
    {
        SceneManager.LoadScene(target);
        if (target != "Map")
        {
            if(Mapula != null)
            {
                mapulaTransform = Mapula.gameObject.transform.position;
            }
        }
        if (target == "Map")
        {

            StartCoroutine(CheckClouds());
            animator.SetTrigger("Hide");
        }
    }

    IEnumerator PlayerTransformSet()
    {
        temp = (LevelData)table.Levels[target];
        yield return new WaitForSeconds(.1f);
        print(temp.GetRight());
        if (temp.GetRight())
        {
            playerLevelTransform = temp.rightSpawn;
        }
        else
        {
            playerLevelTransform = temp.leftSpawn;
        }
        animator.SetTrigger("Hide");
    }
    IEnumerator CheckClouds()
    {
        if (cloudChecking == false)
        {
            cloudChecking = true;

            yield return new WaitForSeconds(.1f);

            MapFog[] clouds = FindObjectsOfType<MapFog>();
            foreach (MapFog cloud in clouds)
            {
                if (mapFogTable.clouds.ContainsKey(cloud.gameObject.name))
                {
                    Destroy(cloud.gameObject);
                }
            }
            cloudChecking = false;
        }
    }

    public void CheckData(LevelData Level)
    {
        if (table.Levels.ContainsKey(Level.levelID))
        {
            //TODO check if this redundance is necessary
            temp = (LevelData)table.Levels[Level.levelID];
            foreach(EnemyManager guy in Level.Entities)
            {
                if (guy.important == true)
                {
                    if (temp.Entities[guy.EnemyID].dead == true)
                    {
                        Destroy(guy.guy.gameObject);
                    }
                }
            }
            foreach (EnemyManager inter in Level.Interactables)
            {

                if (temp.Interactables[inter.EnemyID].dead == true)
                {
                    Destroy(inter.guy.gameObject);
                }
            }
            foreach(LockManager locked in Level.Doors)
            {
                if (temp.Doors[locked.GetID()].GetClosed() == false)
                {
                    locked.GetGuy().GetComponent<Door>().SetClosed(false);
                }
            }
        }   
        else
        {
            print("tthis?");
            table.Levels.Add(Level.levelID, Level);
            print(table.Levels.Count);
        }
        target = Level.levelID;
        StartCoroutine(PlayerTransformSet());
    }



    public void markDead(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
        if (enemies.Enemies.ContainsKey(temp.Entities[ID].myName) == false)
        {
            enemies.Enemies.Add(temp.Entities[ID].myName, temp.Entities[ID].skills);
        }
        temp.Entities[ID].dead = true;
        table.Levels[levelID] = temp;
    }

    public void markDestroyed(int ID, string levelID)
    { 
        temp = (LevelData)table.Levels[levelID];
        temp.Interactables[ID].dead = true;
        table.Levels[levelID] = temp;
    }

    public void MarkOpened(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
        temp.Doors[ID].SetClosed(false);
        table.Levels[levelID] = temp;
    }


    public void LoadAlert(string levelID)
    {
        if (table.Levels.ContainsKey(levelID) == false)
        {
            alertBox.SetActive(true);
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(alertBox.GetComponentInChildren<Button>().gameObject);

        }
       
    }

    public void HideAlert()
    {
        Time.timeScale = 1;
        alertBox.SetActive(false);
    }

    //Setters And Getters

    public void SetMapula(Vector3 vector)
    {
        mapulaTransform = vector;
    }

    public Vector3 GetMapula()
    {
        return mapulaTransform;
    }
}



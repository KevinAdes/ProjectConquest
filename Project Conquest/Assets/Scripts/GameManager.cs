using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Events;

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

    [SerializeField]
    Transform gameOverScreen;

    [SerializeField]
    SkillsList skillsList;

    Vector3 mapulaTransform;
    public MapMovement Mapula;

    public Vector2 playerLevelTransform;

    Animator animator;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    bool singleton = false;

    string target;

    bool right;
    bool cloudChecking = false;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        enemies = ScriptableObject.CreateInstance<EnemyDictionary>();
        table = ScriptableObject.CreateInstance<LevelTable>();
        mapFogTable = ScriptableObject.CreateInstance<MapFogTable>();
        animator = GetComponent<Animator>();
        AddDraculaToEnemiesList();
    }

    private void AddDraculaToEnemiesList()
    {
        List<EnemySkill> DraculaSkills = new List<EnemySkill>();
        foreach(EnemySkill skill in playerData.GetSkills())
        {
            EnemySkill newSkill = Instantiate(skill);
            DraculaSkills.Add(newSkill);
        }
        enemies.Enemies.Add("Dracula", DraculaSkills);
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
            foreach (EnemyManager guy in Level.Entities)
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
            table.Levels.Add(Level.levelID, Level);
        }
        target = Level.levelID;

        if (Level.GetRight())
        {
            playerLevelTransform = Level.rightSpawn;
        }
        else
        {
            playerLevelTransform = Level.leftSpawn;
        }
        animator.SetTrigger("Hide");
        FindObjectOfType<PlayerMovement>().transform.position = playerLevelTransform;
    }

    //MARKER FUNCTIONS////////////
    public void AddSkill(int ID, string levelID)
    {
        print("this is happening");
        temp = (LevelData)table.Levels[levelID];
        if (enemies.Enemies.ContainsKey(temp.Entities[ID].myName) == false)
        {
            enemies.Enemies.Add(temp.Entities[ID].myName, temp.Entities[ID].skills);
        }
    }

    public void markDead(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
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
    /////////////////////////////////
    
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

    public void HideGameOver()
    {
        gameOverScreen.gameObject.SetActive(false);
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

    public SkillsList GetSkills()
    {
        return skillsList;
    }

    public bool GetSingleton()
    {
        return singleton;
    }

}



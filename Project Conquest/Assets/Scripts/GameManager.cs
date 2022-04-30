using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    PlayerData playerData;

    [SerializeField]
    GameObject LoadHider;

    [SerializeField]
    GameObject alertBox;

    [SerializeField]
    Transform gameOverScreen;

    [SerializeField]
    SkillsList skillsList;

    EnemyDictionary enemies;
    LevelTable table;
    MapFogTable mapFogTable;
    LevelData temp;
    StoryFlags flags;


    Vector3 mapulaTransform;
    public MapMovement Mapula;

    public Vector2 playerLevelTransform;

    Animator animator;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    bool singleton = false;

    string target;

    //A bool that determines whether the game should set draculas transform upon entering a level. if dracula is spawning/respawning, it should be deactivated
    bool ignoreDraculaTransform;
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
        flags = ScriptableObject.CreateInstance<StoryFlags>();
        AddDraculaToEnemiesList();
    }

    public void OnEnable()
    {
        animator = GetComponent<Animator>();
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

//VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
//Level Loading Functions

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
        if (table.Levels.ContainsKey(Level.GetID()))
        {
            //TODO check if this redundance is necessary
            temp = (LevelData)table.Levels[Level.GetID()];
            foreach (EnemyManager guy in Level.GetEntities())
            {
                if (guy.GetImportant() == true)
                {
                    if (temp.GetEntities()[guy.GetID()].GetDead() == true)
                    {
                        Destroy(guy.GetGuy().gameObject);
                    }
                }
            }
            foreach (EnemyManager inter in Level.GetInteractables())
            {

                if (temp.GetInteractables()[inter.GetID()].GetDead() == true)
                {
                    Destroy(inter.GetGuy().gameObject);
                }
            }
            foreach(LockManager locked in Level.GetDoors())
            {
                if (temp.GetDoors()[locked.GetID()].GetClosed() == false)
                {
                    locked.GetGuy().GetComponent<Door>().SetClosed(false);
                }
            }
        }   
        else
        {
            table.Levels.Add(Level.GetID(), Level);
        }
        target = Level.GetID();
        if (!ignoreDraculaTransform)
        {
            if (Level.GetRight())
            {
                playerLevelTransform = Level.GetRightSpawn();
            }
            else
            {
                playerLevelTransform = Level.GetLeftSpawn();
            }
            if (FindObjectOfType<PlayerMovement>() != null)
            {
                FindObjectOfType<PlayerMovement>().transform.position = playerLevelTransform;
            }
        }
        animator.SetTrigger("Hide");
    }
    //XXXXXXXXXXXXXXX//////////////////////////////////////

    //VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
    //MARKER FUNCTIONS
    public void AddSkill(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
        if (enemies.Enemies.ContainsKey(temp.GetEntities()[ID].GetMyName()) == false)
        {
            //Starts at one instead of zero to account for dracula
            if (enemies.Enemies.Count == 1)
            {
                FindObjectOfType<Dracula>().GetComponent<CutsceneManager>().ExecuteCutscene();
            }
            enemies.Enemies.Add(temp.GetEntities()[ID].GetMyName(), temp.GetEntities()[ID].GetEnemySkills());
        }
    }

    public void markDead(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
        temp.GetEntities()[ID].SetDead(true);
        table.Levels[levelID] = temp;
    }

    public void markDestroyed(int ID, string levelID)
    { 
        temp = (LevelData)table.Levels[levelID];
        temp.GetInteractables()[ID].SetDead(true);
        table.Levels[levelID] = temp;
    }

    public void MarkOpened(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
        temp.GetDoors()[ID].SetClosed(false);
        table.Levels[levelID] = temp;
    }
    //XXXXXXXXXXXXXXXXXXXXXXXXXXX
    
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

    public bool GetIgnore()
    {
        return ignoreDraculaTransform;
    }

    public void SetIgnore(bool b)
    {
        ignoreDraculaTransform = b;
    }
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

    public StoryFlags GetFlags()
    {
        return flags;
    }

    public EnemyDictionary GetEnemies()
    {
        return enemies;
    }

    public MapFogTable GetMapFog()
    {
        return mapFogTable;
    }

    public LevelTable GetTable()
    {
        return table;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
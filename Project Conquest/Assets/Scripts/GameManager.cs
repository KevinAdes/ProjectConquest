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

    public Vector3 mapulaTransform;
    public MapMovement Mapula;

    public Vector2 playerLevelTransform;

    Animator animator;

    public static GameManager instance;

    string target;

    bool right;
    bool cloudChecking = false;

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

    public void LoadLevelANIMATIONEVENT()
    {
        SceneManager.LoadScene(target);
        if (target != "Map")
        {
            mapulaTransform = Mapula.gameObject.transform.position;
            //StartCoroutine(PlayerTransformSet());
        }
        if (target == "Map")
        {

            StartCoroutine(CheckClouds());
        }
        animator.SetTrigger("Hide");
    }

    IEnumerator PlayerTransformSet()
    {
        yield return new WaitForSeconds(.1f);
        temp = (LevelData)table.Levels[target];
        playerLevelTransform = temp.leftSpawn;
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
            temp = (LevelData)table.Levels[Level.levelID];
            foreach(EnemyManager guy in Level.Entities)
            {
                if (guy.important == true)
                {
                    if (temp.Entities[guy.EnemyID].dead == true)
                    {
                        print("this man is dead, i am killing him" + guy.guy.gameObject.name);
                        Destroy(guy.guy.gameObject);
                    }

                }
            }
        }   
        else
        {
            table.Levels.Add(Level.levelID, Level);
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
}



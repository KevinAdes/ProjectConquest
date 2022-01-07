using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public PlayerData playerData;
    public LevelTable table;
    public MapFogTable mapFogTable;
    public GameObject LoadHider;
    public GameObject alertBox;
    LevelData temp;

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
        playerData = ScriptableObject.CreateInstance<PlayerData>();
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
        StartCoroutine(reload());
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
            StartCoroutine(PlayerTransformSet());
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
            foreach(EnemyManager entity in Level.Entities)
            {
                if (temp.Entities[entity.EnemyID].dead == true)
                {
                    Destroy(entity.guy.gameObject);
                }
            }
        }   
        else
        {
            table.Levels.Add(Level.levelID, Level);
        }
    }

    public void markDead(int ID, string levelID)
    {
        temp = (LevelData)table.Levels[levelID];
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



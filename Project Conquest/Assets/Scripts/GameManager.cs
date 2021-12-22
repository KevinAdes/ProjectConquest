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

    Animator animator;

    public MapMovement Mapula;

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
            Mapula.gameObject.SetActive(false);

        }
        if (target == "Map")
        {
            Mapula.gameObject.SetActive(true);
            StartCoroutine(CheckClouds());
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
            print("its already here...");
            temp = (LevelData)table.Levels[Level.levelID];
            foreach(EnemyManager human in Level.Entities)
            {
                   
                if (temp.Entities[human.EnemyID].dead == true)
                {
                    Destroy(human.guy.gameObject);
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



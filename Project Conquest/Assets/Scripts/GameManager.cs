using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public PlayerData playerData;
    public LevelTable table;
    public GameObject LoadHider;
    LevelData temp;

    public MapMovement Mapula;

    public static GameManager instance;

    bool right;

    private void Awake()
    {
        playerData = ScriptableObject.CreateInstance<PlayerData>();
        table = ScriptableObject.CreateInstance<LevelTable>();
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
        LoadHider.SetActive(true);
        if (ID == "Map")
        {
            Mapula.gameObject.SetActive(true);
        }
        else
        {
            Mapula.gameObject.SetActive(false);

        }
        SceneManager.LoadScene(ID);
        LoadHider.SetActive(false);
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
}



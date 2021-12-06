using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    public LevelTable table;
    LevelData temp;

    public static LevelManager instance;

    private void Awake()
    {
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
        SceneManager.LoadScene(ID);
    }


    public void CheckData(LevelData Level)
    {
        if (table.Levels.ContainsKey(Level.levelID))
        {
            print("i am checking");
            temp = (LevelData)table.Levels[Level.levelID];
            foreach(EnemyManager human in Level.Entities)
            {

                print(human.dead);
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
        print(ID);
        temp = (LevelData)table.Levels[levelID];
        temp.Entities[ID].dead = true;
        table.Levels[levelID] = temp;
        foreach(EnemyManager human in temp.Entities)
        {
            print(human.dead);
        }

    }
}



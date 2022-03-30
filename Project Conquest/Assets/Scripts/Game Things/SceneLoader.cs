using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadIntro()
    {
        if(GetComponent<GameManager>().flags.GameStarted == false)
        {
            SceneManager.LoadScene("Intro Cutscene");
        }
        else
        {
            LoadMap();
        }
    }

    public void Respawn()
    {
        SceneManager.LoadScene("Draculas Lair");
    }
}

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

    public void Respawn()
    {
        SceneManager.LoadScene("Draculas Lair");
    }
}

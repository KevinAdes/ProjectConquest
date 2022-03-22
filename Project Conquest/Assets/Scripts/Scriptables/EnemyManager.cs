using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using static EnemySkills;
using UnityEngine;

public class EnemyManager : ScriptableObject
{
    public string myName;
    public GameObject guy;
    public int EnemyID;
    public bool important = false;
    public bool dead;

    public List<func>skills = new List<func>();

    //skills is created here
    //instantiated in level, where it is given the values of entity.skills. 
}

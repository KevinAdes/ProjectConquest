using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using static EnemySkills;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : ScriptableObject
{
    public string myName;
    public GameObject guy;
    public int EnemyID;
    public bool important = false;
    public bool dead;

    public List<UnityEvent>skills = new List<UnityEvent>();

    public void AddSkill(UnityEvent skillToAdd)
    {
        skills.Add(skillToAdd);
    }

    //create unity event list


    //skills is created here
    //instantiated in level, where it is given the values of entity.skills. 
}

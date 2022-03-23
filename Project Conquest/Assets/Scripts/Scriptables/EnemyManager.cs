using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using static SkillsList;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : ScriptableObject
{
    public string myName;
    public GameObject guy;
    public int EnemyID;
    public bool important = false;
    public bool dead;

    public List<EnemySkill>skills = new List<EnemySkill>();

    public void AddSkill(EnemySkill skillToAdd)
    {
        skills.Add(skillToAdd);
    }

}

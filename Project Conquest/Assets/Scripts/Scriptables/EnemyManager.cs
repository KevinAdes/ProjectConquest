using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static SkillsList;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : ScriptableObject
{
    string myName;
    //TODO give this variable a better name
    GameObject guy;
    int EnemyID;
    //TODO figure out if this important variable is useful
    bool important = false;
    bool dead;

    List<EnemySkill>skills = new List<EnemySkill>();

    public void AddSkill(EnemySkill skillToAdd)
    {
        skills.Add(skillToAdd);
    }
    //Getters and Setters

    public string GetMyName()
    {
        return myName;
    }
    public void SetMyName(string s)
    {
        myName = s;
    }

    public GameObject GetGuy()
    {
        return guy;
    }
    public void SetGuy(GameObject gO)
    {
        guy = gO;
    }

    public int GetID()
    {
        return EnemyID;
    }
    public void SetID(int i)
    {
        EnemyID = i;
    }

    public bool GetImportant()
    {
        return important;
    }
    public void SetImportant(bool b)
    {
        important = b;
    }

    public bool GetDead()
    {
        return dead;
    }
    public void SetDead(bool b)
    {
        dead = b;
    }

    public List<EnemySkill> GetEnemySkills()
    {
        return skills;
    }
}

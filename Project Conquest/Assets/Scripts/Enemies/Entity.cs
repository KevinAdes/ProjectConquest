using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float damage;
    public float defense;
    public int expYield;

    [Header("Components")]
    public Animator animator;

    public bool important;
    public int ID;

    [HideInInspector]
    public bool detected = false;

    [HideInInspector]
    public int direction = 1;


    public float Get_Exp()
    {
        return expYield;
    }

}

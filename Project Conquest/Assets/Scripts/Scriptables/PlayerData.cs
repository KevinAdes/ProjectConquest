using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float maxHealth = 66;
    public float damage = 30;
    public float defense = 26;
    public float attackModifier = 0;

    public float speed = 2;
    public float speedCap = 7;
    public float jump = 5;

    public float blood = 0;
}

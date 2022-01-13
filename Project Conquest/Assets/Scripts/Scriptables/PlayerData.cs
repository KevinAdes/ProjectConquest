using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public float damage;
    public float defense;
    public float attackModifier;

    public float speed;
    public float speedCap;
    public float jump;

    public float blood;
}

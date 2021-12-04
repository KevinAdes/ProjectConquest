using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/BaseEnemy", order = 1)]
public class EnemyManager : ScriptableObject
{
    public string EnemyName;
    bool dead;
    public Transform[] waypoints;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Flags", menuName = "ScriptableObjects/StoryFlags")]

public class StoryFlags : ScriptableObject
{
    //keeping these flags public because making getters and setters for them would be too complicated the way its built now.

    public bool GameStarted;
    public bool TutorialDone;
    public bool Respawn;
    public bool EnemyKilled;
    public bool LockedDoor;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Flags", menuName = "ScriptableObjects/StoryFlags")]

public class StoryFlags : ScriptableObject
{
    public bool GameStarted;
    public bool TutorialDone;
}

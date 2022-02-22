using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Speaker")]
public class NarrationSpeaker : ScriptableObject
{
    [SerializeField]
    private string m_CharacterName;

    public string characterName => m_CharacterName;
}

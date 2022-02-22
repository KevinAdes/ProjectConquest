using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Line")]
public class NarrationLine : ScriptableObject
{
    [SerializeField]
    private NarrationSpeaker m_speaker;
    [SerializeField]
    private string m_text;

    public NarrationSpeaker speaker => m_speaker;
    public string text => m_text;

}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    DialogueChannel channel;

    [SerializeField]
    Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        channel.RaiseRequestDialogue(dialogue);
    }
    
}

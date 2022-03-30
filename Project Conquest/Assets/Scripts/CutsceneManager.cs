using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    bool onAwake;
    
    [SerializeField]
    DialogueChannel channel;

    [SerializeField]
    Dialogue dialogue;

    [SerializeField]
    string flag;

    [SerializeField]
    UnityEvent OtherEvent;

    
    // Start is called before the first frame update
    void Start()
    {
        if (onAwake)
        {
            ExecuteCutscene();
        }
    }

    public void ExecuteCutscene()
    {
        if ((bool)FindObjectOfType<GameManager>().flags.GetType().GetField(flag).GetValue(FindObjectOfType<GameManager>().flags) == false)
        {
            FindObjectOfType<GameManager>().flags.GetType().GetField(flag).SetValue(FindObjectOfType<GameManager>().flags, true);
            channel.RaiseRequestDialogue(dialogue);
        }
        else
        {
            OtherEvent?.Invoke();
        }
    }
}

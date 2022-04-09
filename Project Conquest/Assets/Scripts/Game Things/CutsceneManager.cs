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
    bool onDelay;

    [SerializeField]
    int delay;
    
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
        if (onDelay)
        {
            StartCoroutine(delayStart());
        }
    }

    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(delay);
        ExecuteCutscene();
    }

    public void ExecuteCutscene()
    {
        //An extremely complicated way of finding a variable with a string, checking its value, and then setting it
        if ((bool)FindObjectOfType<GameManager>().GetFlags().GetType().GetField(flag).GetValue(FindObjectOfType<GameManager>().GetFlags()) == false)
        {
            FindObjectOfType<GameManager>().GetFlags().GetType().GetField(flag).SetValue(FindObjectOfType<GameManager>().GetFlags(), true);
            channel.RaiseRequestDialogue(dialogue);
        }
        else if(flag == "")
        {
            channel.RaiseRequestDialogue(dialogue);
        }
        else
        {
            OtherEvent?.Invoke();
        }
    }
}

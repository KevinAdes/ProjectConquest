using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    int priority;

    [SerializeField]
    bool onAwake;

    [SerializeField]
    bool onDelay;

    [SerializeField]
    int delay;

    [SerializeField]
    string[] flags;

    [SerializeField]
    Dialogue[] dialogues;

    [SerializeField]
    DialogueChannel channel;

    [SerializeField]
    UnityEvent OtherEvent;

    //The double dictionary approach may be unnesecary, but it ensures that the flags will be activated in the order that they are set.
    Dictionary<string, int> flagsDict = new Dictionary<string, int>();
    Dictionary<int, Dialogue> dialoguesDict = new Dictionary<int, Dialogue>();

    string flag;
    Dialogue dialogue;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < flags.Length; i++)
        {
            flagsDict.Add(flags[i], i);
            dialoguesDict.Add(i, dialogues[i]);
        }
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
        PickCutscene();
        //An extremely complicated way of finding a variable with a string, checking its value, and then setting it
        print("hello?");
        if ((bool)FindObjectOfType<GameManager>().GetFlags().GetType().GetField(flag).GetValue(FindObjectOfType<GameManager>().GetFlags()) == false)
        {
            print("yellow??");
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

    private void PickCutscene()
    {
        foreach (string temp in flags)
        {
            if ((bool)FindObjectOfType<GameManager>().GetFlags().GetType().GetField(temp).GetValue(FindObjectOfType<GameManager>().GetFlags()) == false)
            {
                flag = temp;
                dialogue = dialoguesDict[flagsDict[temp]];
                return;
            }
        }
    }

}

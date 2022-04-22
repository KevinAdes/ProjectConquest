using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    //TODO move the narration line to only be on nodes that need it
    //WARNING!! BACKUP BEFORE DOING!!! MAY ERASE ALL LINE REFERENCES IN NODES!!!! VERY SCARY!!!!

    [SerializeField]
    private NarrationLine m_DialogueLine;

    public NarrationLine dialogueLine => m_DialogueLine;

    public abstract bool CanBeFollowedByNode(DialogueNode node);

    public abstract void Accept(DialogueNodeVisitor visitor);
}

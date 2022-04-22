using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInstigator : MonoBehaviour
{
    [SerializeField]
    private DialogueChannel m_DialogueChannel;
    //[SerializeField]
    //private FlowChannel m_FlowChannel;
    //[SerializeField]
    //private FlowState m_DialogueState;

    private DialogueSequencer m_DialogueSequencer;
    //private FlowState m_CachedFlowState

    Dracula dracula;
    PlayerMovement playerMovement;
    public Interactable target;

    public void Awake()
    {
        dracula = FindObjectOfType<Dracula>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        m_DialogueSequencer = new DialogueSequencer();

        m_DialogueSequencer.OnDialogueStart += OnDialogueStart;
        m_DialogueSequencer.OnDialogueEnd += OnDialogueEnd;
        m_DialogueSequencer.OnDialogueNodeStart += m_DialogueChannel.RaiseDialogueNodeStart;
        m_DialogueSequencer.OnDialogueNodeEnd += m_DialogueChannel.RaiseDialogueNodeEnd;

        m_DialogueChannel.OnDialogueRequested += m_DialogueSequencer.StartDialogue;
        m_DialogueChannel.OnDialogueNodeRequested += m_DialogueSequencer.StartDialogueNode;
    }

    public void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeRequested -= m_DialogueSequencer.StartDialogueNode;
        m_DialogueChannel.OnDialogueRequested -= m_DialogueSequencer.StartDialogue;

        m_DialogueSequencer.OnDialogueNodeEnd -= m_DialogueChannel.RaiseDialogueNodeEnd;
        m_DialogueSequencer.OnDialogueNodeStart -= m_DialogueChannel.RaiseDialogueNodeStart;
        m_DialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueSequencer.OnDialogueStart -= OnDialogueStart;

        m_DialogueSequencer = null;
    }

    private void OnDialogueStart(Dialogue dialogue)
    {
        dracula?.SetState(states.DIALOGUE);
        playerMovement?.SetState(states.DIALOGUE);
        m_DialogueChannel.RaiseDialogueStart(dialogue);
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {
        m_DialogueChannel.RaiseDialogueEnd(dialogue);
        target?.undoAdditionalAction();
        dracula?.SetState(states.DEFAULT);
        playerMovement?.SetState(states.DEFAULT);

    }
}

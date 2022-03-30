using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Animation")]
public class AnimationDialogueNode : DialogueNode
{
    [SerializeField]
    GameObject AnimatorPrefab;
    [SerializeField]
    string AnimationToPlay;

    Animator animator;

    [SerializeField]
    private DialogueNode m_NextNode;
    public DialogueNode NextNode => m_NextNode;

    public void playAnim()
    {
        animator = GameObject.FindWithTag(AnimatorPrefab.tag).GetComponent<Animator>();
        animator.Play(AnimationToPlay);
    }

    public void GoNextNode(DialogueChannel channel)
    {
        channel.RaiseRequestDialogueNode(NextNode);
    }

    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return m_NextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Animator GetAnimator()
    {
        return animator;
    }

}
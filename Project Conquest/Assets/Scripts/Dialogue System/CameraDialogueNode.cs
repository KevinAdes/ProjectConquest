using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Narration/Dialogue/Node/Camera")]
public class CameraDialogueNode : DialogueNode
{
    [SerializeField]
    private DialogueNode m_NextNode;
    public DialogueNode NextNode => m_NextNode;

    [SerializeField]
    Vector3 coordinates;

    [SerializeField]
    int holdTime;

    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return m_NextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Vector3 GetCoords()
    {
        return coordinates;
    }
    public int GetHold()
    {
        return holdTime;
    }

}
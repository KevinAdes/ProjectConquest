
public interface DialogueNodeVisitor
{
    void Visit(BasicDialogueNode node);
    void Visit(ChoiceDialogueNode node);
    void Visit(ShopDialogueNode node);
    void Visit(AnimationDialogueNode node);
    void Visit(CameraDialogueNode node);
    void Visit(StateSwitchingNode node);
}

using UnityEditor;

[CustomEditor(typeof(PauseControl))]
public class PauseEditor : Editor
{
    SerializedProperty mainTab;
    SerializedProperty itemsTab;
    SerializedProperty equipmentTab;
    SerializedProperty upgradesTab;
    SerializedProperty journalTab;
    SerializedProperty optionsTab;

    SerializedProperty focusButton;
    SerializedProperty itemsButton;
    SerializedProperty equipmentButton;
    SerializedProperty upgradesButton;
    SerializedProperty journalButton;
    SerializedProperty optionsButton;

    SerializedProperty Power;
    SerializedProperty Defense;
    SerializedProperty Speed;
    SerializedProperty Health;
    SerializedProperty Experience;

    bool showTabs, showButtons, showTexts = false;

    public void OnEnable()
    {
        mainTab = serializedObject.FindProperty("mainTab");
        itemsTab = serializedObject.FindProperty("itemsTab");
        equipmentTab = serializedObject.FindProperty("equipmentTab");
        upgradesTab = serializedObject.FindProperty("upgradesTab");
        journalTab = serializedObject.FindProperty("journalTab");
        optionsTab = serializedObject.FindProperty("optionsTab");

        focusButton = serializedObject.FindProperty("focusButton");
        itemsButton = serializedObject.FindProperty("itemsButton");
        equipmentButton = serializedObject.FindProperty("equipmentButton");
        upgradesButton = serializedObject.FindProperty("upgradesButton");
        journalButton = serializedObject.FindProperty("journalButton");
        optionsButton = serializedObject.FindProperty("optionsButton");

        Power = serializedObject.FindProperty("Power");
        Defense = serializedObject.FindProperty("Defense");
        Speed = serializedObject.FindProperty("Speed");
        Health = serializedObject.FindProperty("Health");
        Experience = serializedObject.FindProperty("Experience");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showTabs = EditorGUILayout.Foldout(showTabs, "Tabs");
        if (showTabs)
        {
            EditorGUILayout.PropertyField(mainTab);
            EditorGUILayout.PropertyField(itemsTab);
            EditorGUILayout.PropertyField(equipmentTab);
            EditorGUILayout.PropertyField(upgradesTab);
            EditorGUILayout.PropertyField(journalTab);
            EditorGUILayout.PropertyField(optionsTab);
        }

        showButtons = EditorGUILayout.Foldout(showButtons, "Buttons");
        if (showButtons)
        {
            EditorGUILayout.PropertyField(focusButton);
            EditorGUILayout.PropertyField(itemsButton);
            EditorGUILayout.PropertyField(equipmentButton);
            EditorGUILayout.PropertyField(upgradesButton);
            EditorGUILayout.PropertyField(journalButton);
            EditorGUILayout.PropertyField(optionsButton);
        }

        showTexts = EditorGUILayout.Foldout(showTexts, "Texts");
        if (showTexts)
        {
            EditorGUILayout.PropertyField(Power);
            EditorGUILayout.PropertyField(Defense);
            EditorGUILayout.PropertyField(Speed);
            EditorGUILayout.PropertyField(Health);
            EditorGUILayout.PropertyField(Experience);
        }
        serializedObject.ApplyModifiedProperties();
    }
}

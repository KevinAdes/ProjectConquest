using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [SerializeField]
    private TextMeshProUGUI m_SpeakerText;
    [SerializeField]
    private TextMeshProUGUI m_DialogueText;

    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private RectTransform m_ShopBoxTransform;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private bool m_ListenToInput = false;
    private DialogueNode m_NextNode = null;

    void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
        m_ShopBoxTransform.gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ListenToInput && Input.GetButtonDown("Submit"))
        {
            m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
        }   
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        gameObject.SetActive(true);

        m_DialogueText.text = node.dialogueLine.text;
        m_SpeakerText.text = node.dialogueLine.speaker.characterName;

        node.Accept(this);
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in m_ShopBoxTransform)
        {
            Destroy(child.gameObject);
        }

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
        m_ShopBoxTransform.gameObject.SetActive(false);
        FindObjectOfType<DialogueInstigator>().enabled = true;
    }

    public void Visit(BasicDialogueNode node)
    {
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }
    public void Visit(ChoiceDialogueNode node)
    {
        m_ChoicesBoxTransform.gameObject.SetActive(true);

        foreach(DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            newChoice.Choice = choice;
        }
    }
    public void Visit(ShopDialogueNode node)
    {
        m_ListenToInput = true;
        m_ShopBoxTransform.gameObject.SetActive(true);  
        InventoryObject dummyInventory = ScriptableObject.CreateInstance<InventoryObject>();
        dummyInventory.Container = node.shopInventory.Copy();
        m_ShopBoxTransform.GetComponent<DisplayInventory>().SetInventory(dummyInventory);
    }

    public void SetText(string input)
    {
        m_DialogueText.text = input;
    }
}

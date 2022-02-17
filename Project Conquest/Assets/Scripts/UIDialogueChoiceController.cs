using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueNode m_ChoiceNextNode;

    public DialogueChoice Choice
    {
        set
        {
            m_Choice.text = value.ChoicePreview;
            m_ChoiceNextNode = value.ChoiceNode;
        }
    }

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_DialogueChannel.RaiseRequestDialogueNode(m_ChoiceNextNode);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BloodBank : MonoBehaviour
{
    [SerializeField]
    Transform bankPanel;
    [SerializeField]
    PlayerData data;
    [SerializeField]
    IntStorage bankBlood;
    [SerializeField]
    TextMeshProUGUI bankText;
    [SerializeField]
    TextMeshProUGUI draculaText;

    public void Awake()
    {
        HideDisplay();
    }

    public void ShowDisplay()
    {
        draculaText.text = data.blood.ToString();
        bankText.text = bankBlood.GetInt().ToString();
        FindObjectOfType<PlayerMovement>().StateSwitcher(states.DIALOGUE);
        bankPanel.gameObject.SetActive(true);
    }

    public void HideDisplay()
    {
        FindObjectOfType<PlayerMovement>(true).StateSwitcher(states.DEFAULT);
        bankPanel.gameObject.SetActive(false);
    }

    private void UpdateDisplay()
    {
        draculaText.text = data.blood.ToString();
        bankText.text = bankBlood.GetInt().ToString();
    }

    public void SendToBank()
    {
        if(data.blood > 0)
        {
            bankBlood.SetInt(bankBlood.GetInt() + 1);
            data.blood -= 1;
            UpdateDisplay();
        }
    }

    public void SendToDrac()
    {
        if(bankBlood.GetInt() > 0)
        {
            bankBlood.SetInt(bankBlood.GetInt() - 1);
            data.blood += 1;
            UpdateDisplay();
        }
    }
}

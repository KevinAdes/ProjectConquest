using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StorageUnit : MonoBehaviour
{
    [SerializeField]
    Transform PlayerInventoryPanel;

    [SerializeField]
    Transform ChestInventoryPanel;

    [SerializeField]
    Transform CloseButton;

    public void Awake()
    {
        HideDisplay();
    }

    public void ShowDisplay()
    {
        FindObjectOfType<PlayerMovement>().SetState(states.DIALOGUE);
        CloseButton.gameObject.SetActive(true);
        PlayerInventoryPanel.gameObject.SetActive(true);
        ChestInventoryPanel.gameObject.SetActive(true);
    }

    public void HideDisplay()
    {
        FindObjectOfType<PlayerMovement>(true).SetState(states.DEFAULT);
        CloseButton.gameObject.SetActive(false);
        PlayerInventoryPanel.gameObject.SetActive(false);
        ChestInventoryPanel.gameObject.SetActive(false);
    }

    public void UpdateDisplay()
    {
        PlayerInventoryPanel.GetComponent<DisplayInventory>().UpdateDisplay();
        ChestInventoryPanel.GetComponent<DisplayInventory>().UpdateDisplay();
    }

}

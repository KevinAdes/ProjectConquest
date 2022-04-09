   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    DefenseSystem[] Powering;
    Interactable me;

    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<Interactable>();
    }

    public void PowerDown()
    {
        for (int i = 0; i < Powering.Length; i++)
        {
            if (Powering[i] != null)
            {
                Powering[i].PowerDown();
            }

        }
    }
}

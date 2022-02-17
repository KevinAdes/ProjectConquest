using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Speaker : MonoBehaviour
{
    
    [SerializeField]
    private UnityEvent Converse;

    public void DoAction()
    {
        Converse?.Invoke();
    }
}

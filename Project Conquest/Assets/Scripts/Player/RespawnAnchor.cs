using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAnchor : MonoBehaviour
{
    [SerializeField]
    GameObject dracula;

    public void SpawnDracula()
    {
        GameObject DraculaInst = Instantiate(dracula, transform.position, Quaternion.identity, transform.parent);
    }

    public void Despawn()
    {
        if(FindObjectOfType<Dracula>() != null)
        {
            Destroy(FindObjectOfType<Dracula>().gameObject);
            FindObjectOfType<GameManager>().SetIgnore(true);
        }
    }

}

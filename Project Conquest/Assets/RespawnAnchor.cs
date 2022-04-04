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
        print("creating dracula");
    }

    public void Despawn()
    {
        if(FindObjectOfType<Dracula>() != null)
        {
            print("destroying dracula");
            Destroy(FindObjectOfType<Dracula>().gameObject);
            FindObjectOfType<GameManager>().SetIgnore(true);
        }
    }

}

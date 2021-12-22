using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultTerrain : MonoBehaviour
{

    public float modifier;
    public string TYPE;
    MapMovement Mapula;

    Vector2 respawnOffset;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        TYPE = TYPE.ToUpper();
        Mapula = FindObjectOfType<MapMovement>();    
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Mapula.speed = Mapula.speed * modifier;
        active = true;
        if(TYPE == "WATER")
        {
            GetDir();
            StartCoroutine(Drown());

        }
        
    }

    public void GetDir()
    {
        if (Mapula.transform.position.x > transform.position.x)
        {
            respawnOffset = new Vector2(0, 1);
        }
        else
        {
            respawnOffset = new Vector2(0, -1);
        }
        if (Mapula.transform.position.y > transform.position.y)
        {
            respawnOffset = new Vector2(1, 0);
        }
        else
        {
            respawnOffset = new Vector2(-1, 0);

        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        Mapula.speed = Mapula.speed / modifier;
        active = false;
    }

    IEnumerator Drown()
    {
        Vector2 positionCache = Mapula.transform.position;
        yield return new WaitForSeconds(1.5f);
        //Mapula.GetComponent<Animator>().SetTrigger("Drown");
        if (active == true)
        {
            Mapula.transform.position = positionCache + respawnOffset;
            active = false;

        }
    }
}

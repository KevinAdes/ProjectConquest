using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFog : MonoBehaviour
{

    GameManager Manager;
    Cloud cloudData;


    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Awake()
    {
        Manager = FindObjectOfType<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
        cloudData = ScriptableObject.CreateInstance<Cloud>();
        cloudData.ID = gameObject.name;
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("Entered", true);
            if (!Manager.GetMapFog().clouds.ContainsKey(cloudData.ID))
            {
                Manager.GetMapFog().clouds.Add(cloudData.ID, cloudData);
            }
        }
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

}

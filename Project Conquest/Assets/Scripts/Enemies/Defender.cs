using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender: MonoBehaviour
{

    [HideInInspector]
    public bool vulerable = true;
    Entity me;
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Entity>();
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (me.detected)
        {
            case true:
                break;
            case false:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    PlayerMovement Dracula;

    // Start is called before the first frame update
    void Start()
    {
        Dracula = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostition = new Vector3(Dracula.transform.position.x, Dracula.transform.position.y, this.transform.position.z);
        this.transform.LookAt(targetPostition);
    }
}

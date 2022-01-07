using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{

    [Header("Calculation Variables")]
    public float speed;

    Entity entity;

    float scaleCache;
    Vector3[] waypoints;
    Vector2 velocity = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        scaleCache = transform.localScale.x;
        if (GetComponent<Entity>() != null)
        {
            entity = GetComponent<Entity>();
            speed = entity.speed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Direction();
    }

    private void Direction()
    {
        transform.localScale = new Vector3(scaleCache * entity.direction, transform.localScale.y, transform.localScale.z);
        Vector3 moveVector = new Vector3(entity.speed * entity.direction * Time.deltaTime, 0);
        transform.position += moveVector;
    }


}

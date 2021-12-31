using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    [Header("Components")]
    public Transform Path;


    [Header("Calculation Variables")]
    public float speed;
    public float wait;

    Entity entity;

    int currentPoint = 0;

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
        }
        waypoints = new Vector3[Path.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = Path.GetChild(i).position;
        }

        StartCoroutine(Wander(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        Direction();
    }

    private void Direction()
    {
        bool facingRight = (transform.position.x - waypoints[currentPoint].x <= 0);
        if (!facingRight)
        {
            transform.localScale = new Vector3(scaleCache * -1, transform.localScale.y, transform.localScale.z);
            entity.direction = -1;
        }
        if (facingRight)
        {
            transform.localScale = new Vector3(scaleCache, transform.localScale.y, transform.localScale.z);
            entity.direction = 1;
        }
    }

    IEnumerator Wander(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        currentPoint = 1;
        Vector3 targetWaypoint = waypoints[currentPoint];
        while (true)
        {
            Vector3 positionStore = transform.position;
            var dist = Vector3.Distance(positionStore, targetWaypoint);
            transform.position = Vector2.Lerp(transform.position, targetWaypoint, speed * (Time.deltaTime / dist));
            if (GetComponent<Entity>().detected) { break; }
            if (Mathf.Abs(transform.position.x - targetWaypoint.x) < .1)
            {
                currentPoint = (currentPoint + 1) % waypoints.Length;
                targetWaypoint = waypoints[currentPoint];
                yield return new WaitForSeconds(wait);
            }
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    enum Types {}
    [SerializeField]
    float viewRadius;
    [SerializeField]
    [Range(0,360)]
    float viewAngle;

    [SerializeField]
    float offset;
    [SerializeField]
    LayerMask TargetMask;
    [SerializeField]
    LayerMask Obstacles;

    [SerializeField]
    int direction;

    string TYPE;

    Entity entity;
    Interactable interactable;
    public void Start()
    {
        if (GetComponent<Entity>() != null)
        {
            entity = GetComponent<Entity>();
            TYPE = "ENTITY";
        }
        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
            TYPE = "INTERACTABLE";
        }

    }

    public void Update()
    {
        if (entity != null)
        {
            direction = GetComponent<Entity>().GetDirection();

        }
        FindVisibleTargets();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public void FindVisibleTargets()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, TargetMask);

        foreach (Collider2D target in targetsInViewRadius)
        {

            Transform targetPos = target.transform;
            Vector3 dirToTarget = (targetPos.position - transform.position).normalized;

            if (Vector3.Angle(transform.right * direction,dirToTarget) < viewAngle/2)
            {
                float distace = Vector3.Distance(transform.position, targetPos.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distace, Obstacles)){
                    Init();
                }
            }
            //handles removing targets if they leave the radius
            if((Vector3.Distance(target.transform.position, transform.position)-.5f) > viewRadius)
            {
                switch (TYPE)
                {
                    case "ENTITY":
                        entity.SetDetected(false);
                        break;
                    case "INTERACTABLE":
                        interactable.SetDetected(false);
                        break;
                }
            }
        }
    }

    private void Init()
    {
        switch (TYPE)
        {
            case "ENTITY":
                if (entity.GetDetected() == false)
                {
                    entity.SetDetected(true);
                }
                break;
            case "INTERACTABLE":
                if (interactable.GetDetected() == false)
                {
                    interactable.SetDetected(true);
                }
                break;
        }
    }

    //Getters and Setters
    public float GetViewRadius()
    {
        return viewRadius;
    }
    public float GetViewAngle()
    {
        return viewAngle;
    }
    public int GetDirection()
    {
        return direction;
    }
}

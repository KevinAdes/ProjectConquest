using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask TargetMask;
    public LayerMask Obstacles;

    public int direction;
    //eventually this may have to be changed to allow for non humans to have a field of view.
    HumanController human;

    public void Start()
    {
        human = GetComponent<HumanController>();
    }

    public void Update()
    {
        direction = GetComponent<HumanController>().right;
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
                if (!Physics2D.Raycast(transform.position, dirToTarget, distace, Obstacles) && !human.spooked){
                    human.spooked = true;
                    GetComponent<Animator>().SetTrigger("Spooked");
                }
            }
        }
    }
}

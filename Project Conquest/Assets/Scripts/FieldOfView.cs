using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public float offset;

    public LayerMask TargetMask;
    public LayerMask Obstacles;

    public int direction;

    string TYPE;

    //This Script will be attached to many different game objects and will
    //impact them differently. I don't know if this process can be
    //automated so for now I will add object types to this list and
    //manually select what kind of object has a field of view via the
    //start method. Eventually I would like to make it work so that I don't
    //have to update this script everytime i make a new thing-that-needs-a-field-of-view.

    Turret turret;
    HumanController human;

    public void Start()
    {
        if(GetComponent<HumanController>() != null)
        {
            human = GetComponent<HumanController>();
            TYPE = "HUMAN";
        }
        if(GetComponent<Turret>() != null)
        {
            turret = GetComponent<Turret>();
            TYPE = "TURRET";
        }
    }

    public void Update()
    {
        if (human != null)
        {
            direction = GetComponent<HumanController>().right;

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
                    switch (TYPE)
                    {
                        case "HUMAN":
                            HumanInit();
                            break;
                        case "TURRET":
                            TurretInit();
                            break;
                    }
                }
            }
            //handles removing targets if they leave the radius
            if((Vector3.Distance(target.transform.position, transform.position)-.5f) > viewRadius)
            {
                switch (TYPE)
                {
                    case "HUMAN":
                        break;
                    case "TURRET":
                        turret.detected = false;
                        break;
                }
            }
        }
    }

    private void HumanInit()
    {
        if (human != null && human.spooked == false)
        {
            human.spooked = true;
            GetComponent<Animator>().SetTrigger("Spooked");
        }
    }

    private void TurretInit()
    {
        if (turret != null && turret.detected == false)
        {
            turret.range = viewRadius;
            turret.detected = true;
        }
    }
}

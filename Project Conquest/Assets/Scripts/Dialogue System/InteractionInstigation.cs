using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInstigation : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer Alert;

    List<Interactable> nearbyInteractables = new List<Interactable>();

    public bool HasNearbyInteractables()
    {
        return nearbyInteractables.Count > 0;
    }

    private void Update()
    {
        if (HasNearbyInteractables() && Input.GetButtonDown("Submit") && GetComponent<Dracula>().GetState() == states.DEFAULT)
        {
            Interactable target = null;
            foreach(Interactable interactable in nearbyInteractables)
            {
                if (target == null)
                {
                    target = interactable;
                }
                else
                {
                    if (Vector3.Distance(interactable.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position) && interactable.GetInteraction() != null)
                    {
                        target = interactable;
                    }
                }
                FindObjectOfType<DialogueInstigator>().target = target;
                target.DoAction();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponentInParent<Interactable>();
        if (interactable != null)
        {
            nearbyInteractables.Add(interactable);
            Alert.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponentInParent<Interactable>();
        if (interactable != null)
        {
            nearbyInteractables.Remove(interactable);
            if(nearbyInteractables.Count == 0)
            {
                Alert.enabled = false;
            }
        }
    }

    public List<Interactable> GetInteractables()
    {
        return nearbyInteractables;
    }
}

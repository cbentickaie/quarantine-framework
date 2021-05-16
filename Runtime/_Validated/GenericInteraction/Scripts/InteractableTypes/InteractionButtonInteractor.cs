using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButtonInteractor : Interactable
{
    bool isActive = false;
    [SerializeField]
    List<Interactable> TargetInteractables;
    [SerializeField]
    List<GameObject> TargetGameObjects;
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        if (TargetInteractables[0] != null)
        {
            foreach (Interactable i in TargetInteractables)
            {
                switch (interactionType)
                {
                    case InteractionTypes.GenericInteract:
                        i.StartInteraction();
                        break;

                    case InteractionTypes.EnableDisableObject:
                        i.gameObject.SetActive(!i.gameObject.activeSelf);
                        break;

                    case InteractionTypes.SwapObject:

                        break;
                }
            }
        }
        else
        {
            if (TargetGameObjects[0])
            {
                foreach (GameObject G in TargetGameObjects)
                {
                    switch (interactionType)
                    {
                        case InteractionTypes.GenericInteract:
                            G.GetComponent<Interactable>().StartInteraction();
                            break;

                        case InteractionTypes.EnableDisableObject:
                            G.SetActive(!G.activeSelf);
                            break;

                        case InteractionTypes.SwapObject:

                            break;
                    }
                }
            }
        }
    }

    public override void StopInteraction(bool isPlayer = false)
    {
        base.StopInteraction();
        if (TargetInteractables[0] != null)
        {
            foreach (Interactable i in TargetInteractables)
            {
                switch (interactionType)
                {
                    case InteractionTypes.GenericInteract:
                        i.StopInteraction();
                        break;

                    case InteractionTypes.EnableDisableObject:
                        i.gameObject.SetActive(!i.gameObject.activeSelf);
                        break;

                    case InteractionTypes.SwapObject:

                        break;
                }
            }
        }
        else 
        {
            if (TargetGameObjects[0] != null)
            {
                foreach (GameObject i in TargetGameObjects)
                {
                    switch (interactionType)
                    {
                        case InteractionTypes.GenericInteract:
                            i.GetComponent<Interactable>().StopInteraction();
                            break;

                        case InteractionTypes.EnableDisableObject:
                            i.SetActive(!i.activeSelf);
                            break;

                        case InteractionTypes.SwapObject:

                            break;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "button-finger.png", true);
    }
    private void OnDrawGizmosSelected()
    {
        foreach (Interactable i in TargetInteractables) 
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
        }
    }


}

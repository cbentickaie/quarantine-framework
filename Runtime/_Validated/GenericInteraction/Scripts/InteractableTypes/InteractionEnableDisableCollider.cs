using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEnableDisableCollider : Interactable
{
    public GameObject targetGameObject;
    public override void StartInteraction(bool isPlayer = false)
    {
       // interactionStartedEvent.AddListener(DoExtraThing);
        base.StartInteraction();
        targetGameObject.GetComponent<Collider>().enabled = !targetGameObject.GetComponent<Collider>().enabled;
    }

    void DoExtraThing() 
    {
        print("Extra thing happened");
    }
}

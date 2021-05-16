using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDisableDoorTrigger : Interactable
{
    GameObject targetGameObject;

    private void Awake()
    {
        targetGameObject = gameObject;
    }
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        targetGameObject.GetComponent<Collider>().enabled = false;
    }
}

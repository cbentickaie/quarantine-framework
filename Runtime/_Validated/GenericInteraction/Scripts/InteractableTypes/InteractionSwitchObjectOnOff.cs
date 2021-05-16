using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSwitchObjectOnOff : Interactable
{
    public GameObject targetGameObject;
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        targetGameObject.SetActive(!targetGameObject.activeSelf);
    }
}

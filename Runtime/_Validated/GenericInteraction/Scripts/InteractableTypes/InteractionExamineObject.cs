using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionExamineObject : Interactable
{
    public GameObject targetGameObject;
    public float examineDistance = 0.2f;
    public Transform examinationTransform;
    Vector3 initialPosition;
    Quaternion initialRotation;
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        BeginExamination();

    }

    public override void StopInteraction(bool isPlayer = false)
    {
        base.StopInteraction();
        StopExamination();
    }

    public override void TickInteraction()
    {
        base.TickInteraction();
        //spin object and capture input here
        //set transform and rotation for examination here
        transform.position = (Camera.main.transform.position + (Camera.main.transform.forward* examineDistance));
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    void BeginExamination()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        PlayerController.instance.disablePlayerPawn(true);
    }

    void StopExamination()
    {
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        PlayerController.instance.disablePlayerPawn(false);
    }
}

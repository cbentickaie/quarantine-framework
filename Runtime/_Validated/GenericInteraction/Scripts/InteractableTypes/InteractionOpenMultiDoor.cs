using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOpenMultiDoor : Interactable
{
    //public GameObject targetGameObject;
    public MultiDoor MultiDoorComponent;
    public bool unlockDoorOnInteraction = false;
    [ExecuteInEditMode]
    private void Awake()
    {
        if (!MultiDoorComponent) 
        {
            MultiDoorComponent = GetComponent<MultiDoor>();
        }
        
    }
    private void Start()
    {
       
    }
    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        if (MultiDoorComponent && MultiDoorComponent.requireInput) 
        {
            print("door state");
            if (MultiDoorComponent.isLocked && unlockDoorOnInteraction)
            {
                MultiDoorComponent.UnlockDoor();
            }

            if(!MultiDoorComponent.isLocked)
            //base.StartInteraction();
            if (!MultiDoorComponent.isOpen)
            {
                MultiDoorComponent.OpenDoor();
            }
            else 
            {
                MultiDoorComponent.CloseDoor();
            }
        }        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionStartStopSpinning : Interactable
{
    [SerializeField]
    private bool isSpinning = false;
    public Vector3 SpinRotation = new Vector3(0,1.0f,0);
    public override void StartInteraction(bool isPlayer = false)
    {
        
        base.StartInteraction();
        isSpinning = !isSpinning;
    }

    public override void StopInteraction(bool isPlayer = false)
    {
        base.StopInteraction();
        //isSpinning = false;
    }
    private void Update()
    {
        if (isSpinning)
        {
            gameObject.transform.Rotate(new Vector3(0, 1.0f, 0), Space.Self);
            base.TickInteraction();
        }
    }

}

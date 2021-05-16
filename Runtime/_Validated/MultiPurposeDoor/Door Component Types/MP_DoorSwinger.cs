using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_DoorSwinger : MPDoorComponent {
    public Vector3 doorOpenRotation = new Vector3(0f,90f,0f);
    public bool allowReversal = true;
    Quaternion initialDoorRotation;
    Vector3 tgtRotation;

    private void Awake()
    {
        initialDoorRotation = transform.localRotation;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO:  add 'reversSwing' bool to door update
    public override void UpdateDoorComponent(float doorAlpha, bool reverseDoor)
    {
        if (allowReversal)
        {        
            //base.UpdateDoorComponent(doorAlpha);
            if (reverseDoor)
            {
                tgtRotation = doorOpenRotation * -1;
            }
            else
            {
                tgtRotation = doorOpenRotation;
            }
            transform.localRotation = Quaternion.Slerp(initialDoorRotation, Quaternion.Euler(tgtRotation), doorAlpha);
            //print("door swingin");
            //transform.RotateAround
        }
    }
}

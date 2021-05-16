using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_DoorSlider : MPDoorComponent {
    public Vector3 doorOpenOffset;
    public Vector3 doorInitialOffset;
    private Vector3 doorTargetOffset;
    private void Awake()
    {
        doorInitialOffset = transform.localPosition;
        doorTargetOffset = doorInitialOffset + doorOpenOffset;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void UpdateDoorComponent(float doorAlpha, bool reverseDoor)
    {
        //base.UpdateDoorComponent(doorAlpha);
        transform.localPosition = Vector3.Lerp(doorInitialOffset, doorTargetOffset, doorAlpha);
    }
}

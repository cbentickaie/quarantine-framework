using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPDoorComponent : MonoBehaviour {

    MultiDoor MultiDoorController;


    private void Awake()
    {
        if (GetComponentInParent<MultiDoor>())
        {
            MultiDoorController = GetComponentInParent<MultiDoor>();
            
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public virtual void UpdateDoorComponent(float doorAlpha, bool reverseDoor)

    {

    }
}

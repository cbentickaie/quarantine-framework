using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiDoor : MonoBehaviour {
    public bool isOpen = false;
    public bool requireInput = false;
    public bool isLocked = false;
    public bool closeBehindPlayer = true;
    public float doorSpeed = 1.0f;
    float doorPercentOpen = 0.0f;
    bool reverseDoorAction = false;
    public List<MPDoorComponent> DoorComponents;
    //Events that are triggered when the door changes state

    public UnityEvent DoorOpeningEvent;
    public UnityEvent DoorClosingEvent;
    public UnityEvent DoorLockedEvent;
    public UnityEvent DoorUnlockedEvent;
    private void Awake()
    {
        foreach (MPDoorComponent comp in GetComponentsInChildren<MPDoorComponent>())
        {
            DoorComponents.Add(comp);
        }        
    }

    // Use this for initialization
    void Start ()
    {
        //OpenDoor();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void checkDoorDirection(Collider other)
    {
        float facingDirection = Vector3.Dot(other.transform.forward, transform.forward);
        print("Direction Dot is: " + facingDirection);
        if (facingDirection > 0 && !isOpen)
        {
            reverseDoorAction = true;
        }
        else
        {
            reverseDoorAction = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            checkDoorDirection(other);
        }

        if (other.gameObject.tag == "Player" && !isLocked && !isOpen)
        {
            if (!requireInput)
            {
                OpenDoor();
            }
        } else if (other.gameObject.tag == "Player" && isLocked) 
        {
            DoorLockedEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        

        /*if (other.gameObject.tag == "Player" && !isLocked && Input.GetButtonDown("Fire1"))
        {            
            if (!isOpen)
            {
                checkDoorDirection(other);
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }            
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //checkDoorDirection(other);
        }
        
        if (other.gameObject.tag == "Player" && !isLocked)
        {
            if (closeBehindPlayer)
            {
                CloseDoor();
            }            
        }
    }

    public void OpenDoor()
    {
        StopCoroutine(UpdateDoorClosing());
        StartCoroutine(UpdateDoorOpening());
        DoorOpeningEvent.Invoke();
    }

    public void CloseDoor()
    {
        StopCoroutine(UpdateDoorOpening());
        StartCoroutine(UpdateDoorClosing());
        DoorClosingEvent.Invoke();
    }

    public void UnlockDoor()
    {
        isLocked = false;
        DoorUnlockedEvent.Invoke();
    }

    IEnumerator UpdateDoorOpening()
    {
        
        while (doorPercentOpen < 1)
        {
            
            doorPercentOpen = Mathf.Clamp(doorPercentOpen + (Time.deltaTime * doorSpeed), 0, 1);
            UpdateDoorComponents();
            print(doorPercentOpen);
            yield return null;
        }
        isOpen = true;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator UpdateDoorClosing()
    {
        
        while (doorPercentOpen > 0)
        {

            doorPercentOpen = Mathf.Clamp(doorPercentOpen - Time.deltaTime * doorSpeed, 0, 1);
            UpdateDoorComponents();
            print(doorPercentOpen);
            yield return null;
        }
        isOpen = false;
        yield return new WaitForSeconds(0.1f);
    }
    
    void UpdateDoorComponents()
    {        
        foreach (MPDoorComponent comp in DoorComponents)
        {
            comp.UpdateDoorComponent(doorPercentOpen, reverseDoorAction);
        }
    }

    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "wooden-door.png", true);
        
    }
    private void OnDrawGizmosSelected()
    {

    }
}

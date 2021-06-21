using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BasicDoor : MonoBehaviour
{
    //This script should be applied to a Box Collider set to Trigger.
    //The GameObject your specify as DoorObject in the Inspector will
    //disable collision and disappear when the player is within the Trigger Volume.

    //A Variable we can store describing whether or not the door is OPEN
    bool isDoorOpen = false;
    [SerializeField]GameObject DoorObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Debug.Log("Player Overlapped Door Trigger");
            //open the door IF it is not already open!
            if (!isDoorOpen) 
            {
                DoorObject.SetActive(false);
                isDoorOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Left Door Trigger");
            //Close the door IF it is open!
            if (isDoorOpen)
            {
                DoorObject.SetActive(true);
                isDoorOpen = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Is WITHIN TRIGGER AREA");
            
        }
    }


}
